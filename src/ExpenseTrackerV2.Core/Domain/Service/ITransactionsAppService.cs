using ExpenseTrackerV2.Core.Domain.Dtos.Output;
using ExpenseTrackerV2.Core.Domain.Dtos.Request.Transaction;
using ExpenseTrackerV2.Core.Domain.Entities;
using System;

namespace ExpenseTrackerV2.Core.Domain.Service;

public interface ITransactionsAppService
{
    Task<List<Transactions>> CreateAsync(CreateTrasactionRequest transactionRequest);
    Task PaidAsync(PaidTransactionRequest paidTransactionRequest);
    Task DeleteAsync(long id);
    Task<IPagedResult<FilterByMonthAndYearOutPut>> FilterByMonthAndYearsync(long month, long year, int pageNumber = 1);
    Task<IPagedResult<FilterByMonthAndYearOutPut>> FilterTransactionsByCategoryAsync(string categoryName, string type, long month, long year);
    Task<List<FilterByMonthAndYearOutPut>> FilterByContactAndMonth(long year,long month,string type ,string contactName);
    Task<decimal> FilterExpenseMonthAndYearAsync(long year, long month);
    Task<decimal> FilterIncomeMonthAndYearAsync(long year, long month);
    Task<decimal> GetEconomyAsync(long year, long month);
    Task<List<FilterByMonthAndYearOutPut>> FilterExpenseWithContactAsync(long year, long month);
    //Task<List<FilterByMonthAndYearOutPut>> GetAllTransactionsAsync(long year, long month);
    Task<IPagedResult<FilterByMonthAndYearOutPut>> FilterTransactionByTypeAsync(string type, long month, long year);
}
