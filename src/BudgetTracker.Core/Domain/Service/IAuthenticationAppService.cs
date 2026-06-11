using BudgetTracker.Core.Domain.Entities;
using BudgetTracker.Core.Domain.Models.Output;
using BudgetTracker.Core.Domain.Models.Request.Account;
using System;

namespace BudgetTracker.Core.Domain.Service;

public interface IAuthenticationAppService
{
    Task<string?> SignUpAsync(CreateAccountRequestDto request);
    Task<TokenResponseDto?> SignInAsync(LoginRequestDto request);
    Task<TokenResponseDto?> RefreshTokenAsync(RefreshTokenRequestDto request);
    Task<string> LogOutAsync(long accountId);
    Task<string?> VerifyTokenSignUpAsync(VerifyTokenRequestDto request);
    Task<string?> VerifyEmailAsync(string email);
    Task<string?> ResetPasswordAsync(ResetPasswordRequestDto request);
    Task<string?> ValidateResetCodeAsync(string email, string token);
    Task<TokenResponseDto?> SignInGoogleAsync(GoogleLoginRequestDto request);

}


