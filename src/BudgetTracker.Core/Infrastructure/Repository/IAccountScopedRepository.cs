using BudgetTracker.Core.Domain.Entities;

namespace BudgetTracker.Core.Domain.Repository;

/// <summary>
/// Contract for repositories of account-owned entities. Every operation that
/// targets an existing row requires the owning <c>accountId</c>, so cross-account
/// access is not expressible through this contract.
/// </summary>
public interface IAccountScopedRepository<T> where T : class, IAccountOwned
{
    Task<T> AddAsync(T entity);
    Task<T?> GetByIdAsync(long id, long accountId);
    Task<IEnumerable<T>> GetAllAsync(long accountId);
    Task<bool> UpdateAsync(T entity);
    Task<bool> DeleteAsync(long id, long accountId);
}
