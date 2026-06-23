using BudgetTracker.Core.Domain.Dtos.Output;
using BudgetTracker.Core.Domain.Dtos.Request.Transaction;
using BudgetTracker.Core.Domain.Entities;
using BudgetTracker.Core.Domain.Enum;

namespace BudgetTracker.Core.Domain.Service;

public interface ITransactionsAppService
{
    Task<List<Transactions>> CreateAsync(long accountId, CreateTrasactionRequest transactionRequest);
    Task EditTransactionAsync(long accountId, long id, CreateTrasactionRequest transactionRequest);
    Task PaidAsync(long accountId, PaidTransactionRequest paidTransactionRequest);
    Task DeleteAsync(long accountId, long id);
    Task<IPagedResult<FilterByMonthAndYearOutPut>> FilterByMonthAndYearAsync(long accountId, long month, long year, int pageNumber = 1);
    Task<IPagedResult<FilterByMonthAndYearOutPut>> FilterTransactionsByCategoryAsync(long accountId, Categories categoryName, TypeTransaction type, long month, long year);
    Task<IPagedResult<FilterByMonthAndYearOutPut>> FilterByContactAndMonth(long accountId, long year, long month, TypeTransaction type, long contactId, int pageNumber = 1);
    Task<decimal> FilterExpenseMonthAndYearAsync(long accountId, long year, long month);
    Task<decimal> FilterIncomeMonthAndYearAsync(long accountId, long year, long month);
    Task<decimal> GetEconomyAsync(long accountId, long year, long month);
    Task<List<FilterByMonthAndYearOutPut>> FilterExpenseWithContactAsync(long accountId, long year, long month);
    Task<IPagedResult<FilterByMonthAndYearOutPut>> FilterTransactionByTypeAsync(long accountId, TypeTransaction type, long month, long year);
}


