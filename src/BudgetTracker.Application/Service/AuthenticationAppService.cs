using BudgetTracker.Core.Domain.Entities;
using BudgetTracker.Core.Domain.Models.Output;
using BudgetTracker.Core.Domain.Models.Request.Account;
using BudgetTracker.Core.Domain.Repository;
using BudgetTracker.Core.Domain.Service;
using BudgetTracker.Core.Domain.UnitOfWork;
using BudgetTracker.Core.Domain.Utils;
using BudgetTracker.Core.Infrastructure.Repository;
using BudgetTracker.Core.Infrastructure.Services;
using Microsoft.AspNet.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
namespace BudgetTracker.Application.Service;

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

            if (string.IsNullOrWhiteSpace(request.Password)) throw new ArgumentException("password must not be empty");

            if (request.Password.Length < 8) throw new ArgumentException("password must be at least 8 characters");

            if (request.Password.Length > 20) throw new ArgumentException("password must be less than 20 characters");

            if (request.Password.Any(char.IsUpper) == false) throw new ArgumentException("password must contain at least one uppercase letter");

            if (request.Password.Any(char.IsLower) == false) throw new ArgumentException("password must contain at least one lowercase letter");

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
                EmailVerificationTokenExpiry = DateTime.UtcNow.AddHours(4)
            };

            _unitOfWork.BeginTransaction();
            var savedAccount = await _accountRepository.AddAsync(account);

            var idEncrypted = _passwordHelper.EncryptUrl(savedAccount.Id.ToString());
            await _emailService.SendCodeToEmailAsync(account.Email, idEncrypted, account.EmailVerificationToken);

            _unitOfWork.Commit();

            return "We send a verification email for you";
        }
        catch
        {
            _unitOfWork.Rollback();
            throw;
        }
    }
    public async Task<string?> VerifyTokenSignUpAsync(VerifyTokenRequestDto request)
    {
        try
        {
            var account = await _accountRepository.GetByIdAsync(long.Parse(_passwordHelper.DecryptUrl(request.Id)));

            if (account is null || account.VerifyAttempts > 5)
            {
                throw new ArgumentException("Exceeds attempts");
            }

            if (account.EmailVerificationToken != request.Token)
            {
                account.VerifyAttempts += 1;
                throw new ArgumentException("Invalid Token");
            }

            if (account.EmailVerificationTokenExpiry < DateTime.UtcNow)
            {
                account.VerifyAttempts += 1;
                throw new ArgumentException("Token Expiry");
            }

            account.EmailVerified = true;
            account.EmailVerificationToken = null;
            account.VerifiedAt = DateTime.UtcNow;
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

    public async Task<string?> ValidateResetCodeAsync(string email, string token)
    {
        try {

            var account = await _accountRepository.GetByEmailAsync(email);

            if (account is null) throw new ArgumentException("Account not found");

            var resetPassword = await _resetPasswordRepository.GetByAccountIdAsync(account.Id);

            if(resetPassword is null || resetPassword.ExpireAt < DateTime.UtcNow || resetPassword.HashedToken != token)
            {
                throw new ArgumentException("Invalid Token");
            }

            if(resetPassword.HashedToken != token)
            {
                throw new ArgumentException("Invalid Token");
            }
            var updatedResetPassword = new ResetPassword()
            {
                Id = resetPassword.Id,
                AccountId = resetPassword.AccountId,
                HashedToken = null,
                ExpireAt = DateTime.UtcNow,
                CreatedAt = resetPassword.CreatedAt
            };

            _unitOfWork.BeginTransaction();

            await _resetPasswordRepository.UpdateAsync(updatedResetPassword);

            _unitOfWork.Commit();

            return "Token Validated";
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

            string token = _passwordHelper.GenerateVerificationCode();

            var resetPassword = new ResetPassword()
            {
                AccountId = account.Id,
                HashedToken = token,
                ExpireAt = DateTime.UtcNow.AddHours(1),
                CreatedAt = DateTime.UtcNow,
            };

            var reset = await _resetPasswordRepository.GetByAccountIdAsync(account.Id);

            if (reset is not null)
            {
                reset.ExpireAt = resetPassword.ExpireAt;
                reset.HashedToken = resetPassword.HashedToken;

                _unitOfWork.BeginTransaction();

                await _resetPasswordRepository.UpdateAsync(reset);

                await _emailService.SendVerificationEmailAsync(email, token);

                _unitOfWork.Commit();

                return "Reset email sended";
            }
            else
            {
                _unitOfWork.BeginTransaction();

                await _resetPasswordRepository.AddAsync(resetPassword);

                await _emailService.SendVerificationEmailAsync(email, token);

                _unitOfWork.Commit();

                return "Reset email sended";
            }
        }
        catch
        {
            _unitOfWork.Rollback();
            throw;
        }
    }

    public async Task<string?> ResetPasswordAsync(ResetPasswordRequestDto request)
    {
        try
        {
            var account = await _accountRepository.GetByEmailAsync(request.email);

            if (account is null) throw new KeyNotFoundException("Account not found");

            var resetPassword = await _resetPasswordRepository.GetByAccountIdAsync(account.Id);

            if (resetPassword is null)
            {
                throw new ArgumentException("Invalid Token");
            }

            var hashPassword = new PasswordHasher().HashPassword(request.newPassword);

            account.Password = hashPassword;

            _unitOfWork.BeginTransaction();

            await _accountRepository.UpdateAsync(account);

            _unitOfWork.Commit();

            return "Password Reseted";
        }
        catch
        {
            _unitOfWork.Rollback();
            throw;
        }
    }

    public async Task<TokenResponseDto?> SignInAsync(LoginRequestDto request)
    {
        try
        {
            var account = await _accountRepository.GetByEmailAsync(request.Email);

            if (account == null) throw new Exception("email is invalid");

            if (account.VerifyAttempts > 5) throw new Exception("exceeds attempts");

            if (new PasswordHasher().VerifyHashedPassword(account.Password, request.Password) == PasswordVerificationResult.Failed)
            {
                throw new Exception("password is invalid");
            }

            if (account.IsActive == false) throw new Exception("account is not active");

            if (account.EmailVerified == false) throw new Exception("email is not verified");

            return new TokenResponseDto()
            {
                AccessToken = CreateToken(account),
                RefreshToken = await GenerateAndSaveRefreshToken(account)
            };
        }
        catch
        {
            throw;
        }
    }

    public async Task<string> LogOutAsync(long accountId)
    {
        try
        {
            var account = await _accountRepository.GetByIdAsync(accountId);

            if (account is null)
                throw new Exception("account not found");

            account.RefreshToken = null;
            account.RefreshTokenExpiryTime = null;

            _unitOfWork.BeginTransaction();
            await _accountRepository.UpdateAsync(account);
            _unitOfWork.Commit();

            return "logged out successfully";
        }
        catch
        {
            _unitOfWork.Rollback();
            throw;
        }
    }

    public async Task<TokenResponseDto?> RefreshTokenAsync(RefreshTokenRequestDto request)
    {
        var account = await _accountRepository.GetByIdAsync(request.AccountId);

        if (account is null) throw new Exception("invalid refresh token");

        if (account.RefreshToken != request.RefreshToken || account.RefreshTokenExpiryTime <= DateTime.UtcNow)
        {
            // Invalida o token mesmo quando rejeitado, para evitar reuso
            try
            {
                account.RefreshToken = null;
                account.RefreshTokenExpiryTime = null;
                _unitOfWork.BeginTransaction();
                await _accountRepository.UpdateAsync(account);
                _unitOfWork.Commit();
            }
            catch
            {
                _unitOfWork.Rollback();
                throw;
            }
            throw new Exception("invalid refresh token");
        }

        // Token válido — rotaciona (limpa o atual, gera um novo)
        try
        {
            account.RefreshToken = null;
            account.RefreshTokenExpiryTime = null;
            _unitOfWork.BeginTransaction();
            await _accountRepository.UpdateAsync(account);
            _unitOfWork.Commit();
        }
        catch
        {
            _unitOfWork.Rollback();
            throw;
        }

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
        try
        {
            var refreshToken = _passwordHelper.GenerateRefreshToken();

            account.RefreshToken = refreshToken;
            account.RefreshTokenExpiryTime = DateTime.UtcNow.AddMinutes(configuration.GetValue<int>("Jwt:RefreshTokenExpirationMinutes"));

            _unitOfWork.BeginTransaction();
            await _accountRepository.UpdateAsync(account);
            _unitOfWork.Commit();

            return refreshToken;
        }
        catch
        {
            _unitOfWork.Rollback();
            throw;
        }
    }
}


