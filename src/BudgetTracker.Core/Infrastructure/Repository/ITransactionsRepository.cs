using BudgetTracker.Core.Domain.Dtos.Output;
using BudgetTracker.Core.Domain.Entities;
using System;

namespace BudgetTracker.Core.Domain.Repository;

public interface ITransactionsRepository : IAccountScopedRepository<Transactions>
{
    Task<IPagedResult<Transactions>> FilterTransactionsByCategoryAsync(long accountId, string categoryName, string type, long month, long year, int pageNumber = 1);
    Task<IPagedResult<Transactions>> FilterByMonthAndYearAsync(long accountId, long month, long year, int pageNumber = 1);
    Task<IPagedResult<Transactions>> FilterByContactAsync(long accountId, long year, long month, string type, long contactId, int pageNumber = 1);
    Task<List<Transactions>> FilterExpenseMonthAndYearAsync(long accountId, long year, long month);
    Task<List<Transactions>> FilterIncomeMonthAndYearAsync(long accountId, long year, long month);
    Task<List<Transactions>> FilterExpenseMonthWithContactAsync(long accountId, long year, long month);
    Task<IPagedResult<Transactions>> FilterTransactionsByTypeAsync(long accountId, string type, long month, long year, int pageNumber = 1);

    /// <summary>Atomically flips Paid false→true for an owned transaction. Returns true only for the call that performed the flip.</summary>
    Task<bool> MarkAsPaidAsync(long id, long accountId);

    /// <summary>Total of EXPENSE transactions for a category in the given month/year, scoped to the account.</summary>
    Task<decimal> GetExpenseTotalByCategoryAsync(long accountId, long categoryId, int month, int year);
}


