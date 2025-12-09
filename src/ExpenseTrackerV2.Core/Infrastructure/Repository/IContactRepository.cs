using ExpenseTrackerV2.Core.Domain.Entities;

namespace ExpenseTrackerV2.Core.Domain.Repository;

public interface IContactRepository : IRepositoryBase<Contact>
{
    Task<long> GetByNameAsync(string name);
}
