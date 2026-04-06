using ExpenseTrackerV2.Core.Domain.Dtos.Output;
using ExpenseTrackerV2.Core.Domain.Entities;
using System;

namespace ExpenseTrackerV2.Core.Domain.Repository;

public interface ITransactionsRepository : IRepositoryBase<Transactions>
{
    Task<IPagedResult<Transactions>> FilterTransactionsByCategoryAsync(string categoryName, string type, long month, long year, int pageNumber = 1);
    Task<IPagedResult<Transactions>> FilterByMonthAndYearAsync(long month, long year,int pageNumber = 1);
    Task<List<Transactions>> FilterByMonthAndContactAsync(long year,long month,string type, string contactName);
    Task<List<Transactions>> FilterExpenseMonthAndYearAsync(long year, long month);
    Task<List<Transactions>> FilterIncomeMonthAndYearAsync(long year, long month);
    Task<List<Transactions>> FilterExpenseMonthWithContactAsync(long year, long month);
    Task DeleteTransactionAsync(long id);
    Task<IPagedResult<Transactions>> FilterTransactionsByTypeAsync(string type, long month, long year, int pageNumber = 1);
}
