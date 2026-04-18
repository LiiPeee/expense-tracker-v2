using ExpenseTrackerV2.Core.Domain.Entities;

namespace ExpenseTrackerV2.Core.Domain.Repository;

public interface IAccountRepository : IRepositoryBase<Account>
{
    Task<Account?> GetByEmailAsync(string email);
    Task<Account?> GetByToken(string token);
}
