using ExpenseTrackerV2.Core.Domain.Entities;
using ExpenseTrackerV2.Core.Domain.Repository;
using System;

namespace ExpenseTrackerV2.Infrastructure.Persistence.Repository;

public class TransactionsRepository : RepositoryBase<Transactions>, ITransactionsRepository
{
    public TransactionsRepository(DapperContext context) : base(context)
    {
    }
}
