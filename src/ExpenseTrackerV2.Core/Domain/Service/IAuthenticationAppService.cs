using ExpenseTrackerV2.Core.Domain.Entities;
using ExpenseTrackerV2.Core.Domain.Models.Output;
using ExpenseTrackerV2.Core.Domain.Models.Request.Account;
using System;

namespace ExpenseTrackerV2.Core.Domain.Service;

public interface IAuthenticationAppService
{
    Task<string?> SignUpAsync(CreateAccountRequestDto request);
    Task<TokenResponseDto?> SignInAsync(LoginRequestDto request);
    Task<TokenResponseDto?> RefreshTokenAsync(RefreshTokenRequestDto request);
    Task<string> LogOutAsync(long accountId);
    Task<string?> VerifyTokenAsync(VerifyTokenRequestDto request);
    Task<string?> VerifyEmailAsync(string email);
    Task<string?> ResetPasswordAsync(ResetPasswordRequestDto request);
}
