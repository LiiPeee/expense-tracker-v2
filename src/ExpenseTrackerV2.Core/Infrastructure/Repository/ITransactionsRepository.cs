using ExpenseTrackerV2.Core.Domain.Dtos.Output;
using ExpenseTrackerV2.Core.Domain.Entities;
using System;

namespace ExpenseTrackerV2.Core.Domain.Repository;

public interface ITransactionsRepository : IRepositoryBase<Transactions>
{
    Task<List<Transactions>> FilterTransactionsByCategoryAsync(long categoryId, long month, long year);
    Task<List<Transactions>> FilterByMonthAndYearAsync(long month, long year);
    Task<List<Transactions>> FilterByMonthAndContactAsync(long year,long month, long contactId);
    Task<List<Transactions>> FilterExpenseMonthAndYearAsync(long year, long month);
    Task<List<Transactions>> FilterIncomeMonthAndYearAsync(long year, long month);
    Task<List<Transactions>> FilterExpenseMonthWithContactAsync(long year, long month);
}
