using ExpenseTrackerV2.Core.Domain.Dtos.Output;
using ExpenseTrackerV2.Core.Domain.Entities;
using System;

namespace ExpenseTrackerV2.Core.Domain.Repository;

public interface ITransactionsRepository : IRepositoryBase<Transactions>
{
    Task<IPagedResult<Transactions>> FilterTransactionsByCategoryAsync(long accountId, string categoryName, string type, long month, long year, int pageNumber = 1);
    Task<IPagedResult<Transactions>> FilterByMonthAndYearAsync(long accountId, long month, long year, int pageNumber = 1);
    Task<List<Transactions>> FilterByMonthAndContactAsync(long accountId, long year, long month, string type, string contactName);
    Task<List<Transactions>> FilterExpenseMonthAndYearAsync(long accountId, long year, long month);
    Task<List<Transactions>> FilterIncomeMonthAndYearAsync(long accountId, long year, long month);
    Task<List<Transactions>> FilterExpenseMonthWithContactAsync(long accountId, long year, long month);
    Task DeleteTransactionAsync(long accountId, long id);
    Task<IPagedResult<Transactions>> FilterTransactionsByTypeAsync(long accountId, string type, long month, long year, int pageNumber = 1);
}
