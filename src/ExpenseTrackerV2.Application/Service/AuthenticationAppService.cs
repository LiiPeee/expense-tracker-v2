using ExpenseTrackerV2.Core.Domain.Entities;
using ExpenseTrackerV2.Core.Domain.Models.Output;
using ExpenseTrackerV2.Core.Domain.Models.Request.Account;
using ExpenseTrackerV2.Core.Domain.Repository;
using ExpenseTrackerV2.Core.Domain.Service;
using ExpenseTrackerV2.Core.Domain.UnitOfWork;
using ExpenseTrackerV2.Core.Domain.Utils;
using ExpenseTrackerV2.Core.Infrastructure.Repository;
using ExpenseTrackerV2.Core.Infrastructure.Services;
using Microsoft.AspNet.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
namespace ExpenseTrackerV2.Application.Service;

public class AuthenticationAppService(IAccountRepository accountRepository,
    IResetPasswordRepository resetPasswordRepository,
    IPasswordHelper passwordHelper, 
    IEmailService emailService, 
    IUnitOfWork unitOfWork, 
    IConfiguration configuration) : IAuthenticationAppService
{
    private readonly IAccountRepository _accountRepository = accountRepository;
    private readonly IEmailService _emailService = emailService;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IPasswordHelper _passwordHelper = passwordHelper;

    private readonly IResetPasswordRepository _resetPasswordRepository = resetPasswordRepository;
    public async Task<string?> SignUpAsync(CreateAccountRequestDto request)
    {
        try
        {
            if (await _accountRepository.GetByEmailAsync(request.Email) != null) throw new ArgumentException("account is already exist");

            if (!string.IsNullOrWhiteSpace(request.Password)) throw new ArgumentException("password must not be empty");

            if (request.Password.Length < 8) throw new ArgumentException("password must be at least 8 characters");

            if(request.Password.Length > 20) throw new ArgumentException("password must be less than 20 characters");

            if(request.Password.Any(char.IsUpper) == false) throw new ArgumentException("password must contain at least one uppercase letter");

            if(request.Password.Any(char.IsLower) == false) throw new ArgumentException("password must contain at least one lowercase letter");

            var hashPassword = new PasswordHasher().HashPassword(request.Password);

            var account = new Account
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Password = hashPassword,
                Balance = 0,
                IsActive = false,
                EmailVerified = false,
                VerifiedAt = null,
                EmailVerificationToken = _passwordHelper.GenerateVerificationCode(),
                EmailVerificationTokenExpiry = DateTime.Now.AddHours(4)
            };

            _unitOfWork.BeginTransaction();
            var savedAccount = await _accountRepository.AddAsync(account);

            var idEncrypted = _passwordHelper.EncryptUrl(savedAccount.Id.ToString());

            await _emailService.SendCodeToEmailAsync(account.Email, idEncrypted, account.EmailVerificationToken);

            _unitOfWork.Commit();

            return "We send a verification email for you";
        }
        catch (Exception ex)
        {
            _unitOfWork.Rollback();
            throw;
        }
    }
    public async Task<string?> VerifyTokenAsync(VerifyTokenRequestDto request)
    {
        try
        {
            var account = await _accountRepository.GetByIdAsync(long.Parse(_passwordHelper.DecryptUrl(request.id)));

            if (account is null || account.VerifyAttempts > 5) 
            {
                
                throw new ArgumentException("Excceds attempts"); 
            }

            if (account.EmailVerificationToken != request.Token) 
            {
                account.VerifyAttempts += 1;
                throw new ArgumentException("Invalid Token"); 
            }

            if (account.EmailVerificationTokenExpiry < DateTime.UtcNow) {
                account.VerifyAttempts +=1;
                throw new ArgumentException("Token Expiry"); 
            }

            account.EmailVerified = true;
            account.EmailVerificationToken = null;
            account.VerifiedAt = DateTime.Now;
            account.IsActive = true;

            _unitOfWork.BeginTransaction();
            await _accountRepository.UpdateAsync(account);

            _unitOfWork.Commit();

            return "Your email has been verified successfully";
        }
        catch (Exception ex)
        {
            _unitOfWork.Rollback();
            throw;
        }
    }

    public async Task<string?> VerifyEmailAsync(string email)
    {
        try
        {

            var account = await _accountRepository.GetByEmailAsync(email);
            if (account is null) throw new ArgumentException();

            string passwordResetToken = _passwordHelper.GenerateRefreshToken();

            var combinedToken = $"{passwordResetToken}|{account.Id}";
            var encryptedToken = _passwordHelper.Encrypt(combinedToken);

            var resetPassword = new ResetPassword()
            {
                AccountId = account.Id,
                HashedToken = passwordResetToken,
                ExpireAt = DateTime.UtcNow.AddHours(1),
                CreatedAt = DateTime.UtcNow,
            };
            _unitOfWork.BeginTransaction();

            await _resetPasswordRepository.AddAsync(resetPassword);

            await _emailService.SendVerificationEmailAsync(email, encryptedToken);

            _unitOfWork.Commit();

            return "Reset email sended";
        }
        catch (Exception ex)
        { 
            throw ex; 
        }
    }

    public async Task<string?> ResetPasswordAsync(ResetPasswordRequestDto request)
    {
        try
        {

            var decryptedToken = _passwordHelper.Decrypt(request.Token);

            var tokenParts = decryptedToken.Split('|');

            if (tokenParts.Length != 2) 
            {
                 throw new KeyNotFoundException("Invalid Token");
            }

            var passwordResetToken = tokenParts[0];
            var accountId = long.Parse(tokenParts[1]);

            var account = await _accountRepository.GetByIdAsync(accountId);

            if (account is null) throw new KeyNotFoundException("Account not found");

            var resetPassword = await _resetPasswordRepository.GetByAccountIdAsync(accountId);
            
            if(resetPassword is null || resetPassword.ExpireAt < DateTime.UtcNow)
            {
                throw new ArgumentException("Invalid Token");
            }

            var hashPassword = new PasswordHasher().HashPassword(request.NewPassword);

            account.Password = hashPassword;

            _unitOfWork.BeginTransaction();

            await _accountRepository.UpdateAsync(account);

            _unitOfWork.Commit();

            return "Password Reseted";
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public async Task<TokenResponseDto?> SignInAsync(LoginRequestDto request)
    {
        try
        {
            var account = await _accountRepository.GetByEmailAsync(request.Email);

            if (account == null) throw new Exception("email is invalid");

            if(account.VerifyAttempts > 5) throw new Exception("exceeds attempts");

            if (new PasswordHasher().VerifyHashedPassword(account.Password, request.Password) == PasswordVerificationResult.Failed)
            {
                throw new Exception("password is invalid");
            }

            if(account.IsActive == false) throw new Exception("account is not active");

            if(account.EmailVerified == false) throw new Exception("email is not verified");

            return new TokenResponseDto()
            {
                AccessToken = CreateToken(account),
                RefreshToken = await GenerateAndSaveRefreshToken(account)
            };
        }
        catch (Exception error)
        {
            throw error;
        }
    }

    public async Task<string> LogOutAsync(long accountId)
    {
        var account = await _accountRepository.GetByIdAsync(accountId);
        _unitOfWork.BeginTransaction();

        if (account != null)
        {
            account.RefreshToken = null;
            account.RefreshTokenExpiryTime = null;
            await _accountRepository.UpdateAsync(account);

            _unitOfWork.Commit();

            return "logged out successfully";
        }
        throw new Exception("account not found");
    }

    public async Task<TokenResponseDto?> RefreshTokenAsync(RefreshTokenRequestDto request)
    {
        _unitOfWork.BeginTransaction();

        var account = await _accountRepository.GetByIdAsync(request.AccountId);

        if (account is null) throw new Exception("invalid refresh token");

        await ValidateRefreshTokenAsync(account, request.RefreshToken);

        account.RefreshToken = null;
        account.RefreshTokenExpiryTime = null;
        await _accountRepository.UpdateAsync(account);

        _unitOfWork.Commit();

        return new TokenResponseDto()
        {
            AccessToken = CreateToken(account),
            RefreshToken = await GenerateAndSaveRefreshToken(account)
        };
    }

    private string CreateToken(Account account)
    {
        var claims = new List<Claim>
            {
                new(ClaimTypes.Name, account.Email),
                new(ClaimTypes.NameIdentifier, account.Id.ToString()),
                new(ClaimTypes.Role, account.Role)
            };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetValue<string>("Jwt:Token")));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

        var tokenDescripton = new JwtSecurityToken(
            issuer: configuration.GetValue<string>("Jwt:Issuer"),
            audience: configuration.GetValue<string>("Jwt:Audience"),
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(configuration.GetValue<int>("Jwt:TokenExpirationMinutes")),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(tokenDescripton);
    }

    private async Task<string> GenerateAndSaveRefreshToken(Account account)
    {
        _unitOfWork.BeginTransaction();

        var refreshToken = _passwordHelper.GenerateRefreshToken();

        account.RefreshToken = refreshToken;
        account.RefreshTokenExpiryTime = DateTime.UtcNow.AddMinutes(configuration.GetValue<int>("Jwt:RefreshTokenExpirationMinutes"));

        await _accountRepository.UpdateAsync(account);

        _unitOfWork.Commit();
        return refreshToken;

    }

    private async Task<Account?> ValidateRefreshTokenAsync(Account account, string refreshToken)
    {
        _unitOfWork.BeginTransaction();

        if (account.RefreshToken != refreshToken || account.RefreshTokenExpiryTime <= DateTime.UtcNow)
        {
            account.RefreshToken = null;
            account.RefreshTokenExpiryTime = null;

            await _accountRepository.UpdateAsync(account);

            _unitOfWork.Commit();
            throw new Exception("invalid refresh token");
        }
        return account;
    }
}
