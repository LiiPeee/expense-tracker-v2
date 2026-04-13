using ExpenseTrackerV2.Core.Domain.Entities;
using ExpenseTrackerV2.Core.Domain.Models.Output;
using ExpenseTrackerV2.Core.Domain.Models.Request.Account;
using System;

namespace ExpenseTrackerV2.Core.Domain.Service;

public interface IAuthenticationAppService
{
    Task<CreateAccountDto?> CreateAsync(CreateAccountRequest request);
    Task<TokenResponseDto?> LoginAsync(LoginRequest request);
    Task<TokenResponseDto?> RefreshTokenAsync(RefreshTokenRequestDto request);
    Task<string> LogOutAsync(long accountId);
}
