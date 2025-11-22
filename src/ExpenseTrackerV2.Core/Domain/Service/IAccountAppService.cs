using System;
using ExpenseTrackerV2.Application.Dtos.Request;

namespace ExpenseTrackerV2.Core.Domain.Service;

public interface IAccountAppService
{
    Task CreateAsync(AccountRequest request);
}
