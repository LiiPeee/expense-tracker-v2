using ExpenseTrackerV2.Core.Domain.Entities;

namespace ExpenseTrackerV2.Core.Domain.Repository;

public interface IContactRepository : IRepositoryBase<Contact>
{
    Task<Contact?> GetByNameAsync(long accountId, string name);
    Task<List<Contact?>> GetByIdAccount(long accountId);
}
