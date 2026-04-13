using ExpenseTrackerV2.Core.Domain.Entities;
using ExpenseTrackerV2.Core.Domain.Models.Output;
using ExpenseTrackerV2.Core.Domain.Models.Request.Account;
using ExpenseTrackerV2.Core.Domain.Repository;
using ExpenseTrackerV2.Core.Domain.Service;
using ExpenseTrackerV2.Core.Domain.UnitOfWork;
using Microsoft.AspNet.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
namespace ExpenseTrackerV2.Application.Service;

public class AuthenticationAppService(IAccountRepository accountRepository, IUnitOfWork unitOfWork, IConfiguration configuration) : IAuthenticationAppService
{
    private readonly IAccountRepository _accountRepository = accountRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<CreateAccountDto?> CreateAsync(CreateAccountRequest request)
    {
        if (await _accountRepository.GetByEmailAsync(request.Email) != null) throw new Exception("account is already exist");

        var hashPassword = new PasswordHasher().HashPassword(request.Password);

        var account = new Account
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            Password = hashPassword,
            Balance = 0,
        };

        var accountResponse = await _accountRepository.AddAsync(account);

        return new CreateAccountDto()
        {
            Name = accountResponse.FirstName,
            Email = accountResponse.Email
        };
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

        var keyToken = configuration.GetValue<string>("Jwt:Token");
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

    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    private async Task<string> GenerateAndSaveRefreshToken(Account account)
    {
        var refreshToken = GenerateRefreshToken();

        account.RefreshToken = refreshToken;
        account.RefreshTokenExpiryTime = DateTime.UtcNow.AddMinutes(configuration.GetValue<int>("Jwt:RefreshTokenExpirationMinutes"));

        await _accountRepository.UpdateAsync(account);

        _unitOfWork.Commit();
        return refreshToken;

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
