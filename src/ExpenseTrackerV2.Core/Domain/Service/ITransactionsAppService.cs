using ExpenseTrackerV2.Core.Domain.Dtos.Output;
using ExpenseTrackerV2.Core.Domain.Dtos.Request.Transaction;
using ExpenseTrackerV2.Core.Domain.Entities;
using System;

namespace ExpenseTrackerV2.Core.Domain.Service;

public interface ITransactionsAppService
{
    Task<List<Transactions>> CreateAsync(CreateTrasactionRequest transactionRequest);
    Task<List<FilterByMonthAndCategory>> FilterTransactionsByCategoryAsync(long categoryId, long month);
    Task PaidAsync(PaidTransactionRequest paidTransactionRequest);
    Task<List<FilterByMonthAndCategory>> FilterByMonthAsync(long month);
}
