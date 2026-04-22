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
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
namespace ExpenseTrackerV2.Application.Service;

public class AuthenticationAppService(IAccountRepository accountRepository,IResetPasswordRepository resetPasswordRepository, IEmailService emailService, IUnitOfWork unitOfWork, IConfiguration configuration) : IAuthenticationAppService
{
    private readonly IAccountRepository _accountRepository = accountRepository;
    private readonly IEmailService _emailService = emailService;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IResetPasswordRepository _resetPasswordRepository = resetPasswordRepository;
    public async Task<string?> CreateAsync(CreateAccountRequest request)
    {
        try
        {
            _unitOfWork.BeginTransaction();

            if (await _accountRepository.GetByEmailAsync(request.Email) != null) throw new ArgumentException("account is already exist");

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
                EmailVerificationToken = GenerateVerificationCode(),
                EmailVerificationTokenExpiry = DateTime.Now.AddHours(4)
            };

            await _accountRepository.AddAsync(account);

            await _emailService.SendCodeToEmailAsync(account.Email, account.EmailVerificationToken);

            _unitOfWork.Commit();

            return "We send a verification email for you";
        }
        catch (Exception ex)
        {
            _unitOfWork.Rollback();
            throw;
        }
    }
    public async Task<string?> VerifyTokenAsync(string token)
    {
        try
        {
            _unitOfWork.BeginTransaction();

            var account = await _accountRepository.GetByToken(token);

            if (account is null) throw new ArgumentException("Invalid Token");

            account.EmailVerified = true;
            account.EmailVerificationToken = null;
            account.VerifiedAt = DateTime.Now;
            account.IsActive = true;

            await _accountRepository.UpdateAsync(account);

            _unitOfWork.Commit();

            return "Email verified successfully";
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
            _unitOfWork.BeginTransaction();

            var account = await _accountRepository.GetByEmailAsync(email);
            if (account is null) throw new ArgumentException();

            string passwordResetToken = PasswordHelper.GenerateRefreshToken();

            var combinedToken = $"{passwordResetToken}|{account.Id}";
            var encryptedToken = PasswordHelper.Encrypt(combinedToken);

            var resetPassword = new ResetPassword()
            {
                AccoountId = account.Id,
                HashedToken = passwordResetToken,
                ExpireAt = DateTime.UtcNow.AddHours(1)
            };

            await  _resetPasswordRepository.AddAsync(resetPassword);

            await _emailService.SendVerificationEmailAsync(email, encryptedToken);

            _unitOfWork.Commit();

            return "Reset email sended";
        }
        catch (Exception ex)
        { 
            throw ex; 
        }
    }

    public async Task<string?> ResetPasswordAsync(ResetPasswordRequest request)
    {
        try
        {
            _unitOfWork.BeginTransaction();

            var decryptedToken = PasswordHelper.Decrypt(request.Token);

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

            var hashPassword = new PasswordHasher().HashPassword(request.NewPassword);

            account.Password = hashPassword;

            await _accountRepository.UpdateAsync(account);

            _unitOfWork.Commit();

            return "Password Reseted";
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public async Task<TokenResponseDto?> LoginAsync(LoginRequest request)
    {
        try
        {
            var account = await _accountRepository.GetByEmailAsync(request.Email);

            if (account == null) throw new Exception("email is invalid");

            if (new PasswordHasher().VerifyHashedPassword(account.Password, request.Password) == PasswordVerificationResult.Failed)
            {
                throw new Exception("password is invalid");
            }

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

        var account = await ValidateRefreshTokenAsync(request.AccountId, request.RefreshToken);

        if (account is null) throw new Exception("invalid refresh token");

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

        var refreshToken = PasswordHelper.GenerateRefreshToken();

        account.RefreshToken = refreshToken;
        account.RefreshTokenExpiryTime = DateTime.UtcNow.AddMinutes(configuration.GetValue<int>("Jwt:RefreshTokenExpirationMinutes"));

        await _accountRepository.UpdateAsync(account);

        _unitOfWork.Commit();
        return refreshToken;

    }
    private static string GenerateVerificationCode()
    {
        Random random = new Random();
        return random.Next(100000, 999999).ToString();
    }

    private async Task<Account?> ValidateRefreshTokenAsync(long accountId, string refreshToken)
    {
        var account = await _accountRepository.GetByIdAsync(accountId);
        if (account == null || account.RefreshToken != refreshToken || account.RefreshTokenExpiryTime <= DateTime.UtcNow)
        {
            throw new Exception("invalid refresh token");
        }
        return account;
    }
}
