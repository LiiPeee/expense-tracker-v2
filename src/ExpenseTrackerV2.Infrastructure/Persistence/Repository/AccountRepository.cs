using ExpenseTrackerV2.Core.Domain.Entities;
using ExpenseTrackerV2.Core.Domain.Repository;
using System.Data;

namespace ExpenseTrackerV2.Infrastructure.Persistence.Repository;

public class AccountRepository : RepositoryBase<Account>, IAccountRepository
{
    public AccountRepository(DbSession connection) : base(connection)
    {
    }
}