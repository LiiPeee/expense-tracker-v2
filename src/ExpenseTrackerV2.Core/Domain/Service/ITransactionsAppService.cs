using ExpenseTrackerV2.Core.Domain.Dtos.Output;
using ExpenseTrackerV2.Core.Domain.Dtos.Request.Transaction;
using ExpenseTrackerV2.Core.Domain.Entities;
using System;

namespace ExpenseTrackerV2.Core.Domain.Service;

public interface ITransactionsAppService
{
    Task<List<Transactions>> CreateAsync(CreateTrasactionRequest transactionRequest);
    Task PaidAsync(PaidTransactionRequest paidTransactionRequest);
    Task<List<FilterByMonthOutPut>> FilterByMonthAndYearsync(long month, long year);
    Task<List<FilterByMonthAndCategoryOutPut>> FilterTransactionsByCategoryAsync(long categoryId, long month, long year);
    Task<List<FilterByContactAndMonthOutPut>> FilterByContactAndMonth(long year,long month, long contactId);
    Task<decimal> FilterExpenseMonthAndYearAsync(long year, long month);
    Task<decimal> FilterIncomeMonthAndYearAsync(long year, long month);
    Task<List<FilterByContactAndMonthOutPut>> FilterExpenseWithContactAsync(long year, long month);
}
