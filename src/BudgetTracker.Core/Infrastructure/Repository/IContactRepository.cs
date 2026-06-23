using BudgetTracker.Core.Domain.Entities;

namespace BudgetTracker.Core.Domain.Repository;

public interface IContactRepository : IAccountScopedRepository<Contact>
{
    Task<Contact?> GetByNameAsync(long accountId, string name);
    Task<List<Contact?>> GetByIdAccount(long accountId);
}


