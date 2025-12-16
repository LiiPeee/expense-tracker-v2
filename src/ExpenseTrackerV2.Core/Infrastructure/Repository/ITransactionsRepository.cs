using ExpenseTrackerV2.Core.Domain.Dtos.Output;
using ExpenseTrackerV2.Core.Domain.Entities;
using System;

namespace ExpenseTrackerV2.Core.Domain.Repository;

public interface ITransactionsRepository : IRepositoryBase<Transactions>
{
    Task<List<Transactions>> FilterTransactionsByCategoryAsync(long categoryId, long month);
    Task<List<Transactions>> FilterByMonthAsync(long month);
}
