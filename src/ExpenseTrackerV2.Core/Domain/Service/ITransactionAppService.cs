using ExpenseTrackerV2.Application.Dtos.Request;
using ExpenseTrackerV2.Core.Domain.Entities;
using System;

namespace ExpenseTrackerV2.Core.Domain.Service;

public interface ITransactionAppService
{
    Task<Transactions> CreateAsync(TransactionRequest transactionRequest);
}
