using ExpenseTrackerV2.Core.Domain.Dtos.Output;
using ExpenseTrackerV2.Core.Domain.Entities;
using System;

namespace ExpenseTrackerV2.Core.Domain.Repository;

public interface ITransactionsRepository : IRepositoryBase<Transactions>
{
    Task<List<Transactions>> FilterTransactionsByCategoryAsync(long categoryId, long month);
    Task<List<Transactions>> FilterByMonthAndYearAsync(long month, long year);
    Task<List<Transactions>> FilterByMonthAndContact(long month, long contactId);
}
