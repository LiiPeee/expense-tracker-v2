using ExpenseTrackerV2.Core.Domain.Dtos.Output;
using ExpenseTrackerV2.Core.Domain.Entities;
using System;

namespace ExpenseTrackerV2.Core.Domain.Repository;

public interface ITransactionsRepository : IRepositoryBase<Transactions>
{
    Task<List<Transactions>> FilterTransactionsByCategoryAsync(long categoryId, long month, long year);
    Task<List<Transactions>> FilterByMonthAndYearAsync(long month, long year);
    Task<List<Transactions>> FilterByMonthAndContact(long year,long month, long contactId);
    Task<List<Transactions>> FilterExpenseMonthAndYear(long year, long month);
    Task<List<Transactions>> FilterIncomeMonthAndYear(long year, long month);
}
