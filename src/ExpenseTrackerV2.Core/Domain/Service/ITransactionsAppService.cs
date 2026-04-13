using ExpenseTrackerV2.Core.Domain.Dtos.Output;
using ExpenseTrackerV2.Core.Domain.Dtos.Request.Transaction;
using ExpenseTrackerV2.Core.Domain.Entities;
using ExpenseTrackerV2.Core.Domain.Enum;

namespace ExpenseTrackerV2.Core.Domain.Service;

public interface ITransactionsAppService
{
    Task<List<Transactions>> CreateAsync(long accountId, CreateTrasactionRequest transactionRequest);
    Task PaidAsync(long accountId, PaidTransactionRequest paidTransactionRequest);
    Task DeleteAsync(long accountId, long id);
    Task<IPagedResult<FilterByMonthAndYearOutPut>> FilterByMonthAndYearsync(long accountId, long month, long year, int pageNumber = 1);
    Task<IPagedResult<FilterByMonthAndYearOutPut>> FilterTransactionsByCategoryAsync(long accountId, Categories categoryName, TypeTransaction type, long month, long year);
    Task<List<FilterByMonthAndYearOutPut>> FilterByContactAndMonth(long accountId, long year, long month, TypeTransaction type, string contactName);
    Task<decimal> FilterExpenseMonthAndYearAsync(long accountId, long year, long month);
    Task<decimal> FilterIncomeMonthAndYearAsync(long accountId, long year, long month);
    Task<decimal> GetEconomyAsync(long accountId, long year, long month);
    Task<List<FilterByMonthAndYearOutPut>> FilterExpenseWithContactAsync(long accountId, long year, long month);
    Task<IPagedResult<FilterByMonthAndYearOutPut>> FilterTransactionByTypeAsync(long accountId, TypeTransaction type, long month, long year);
}
