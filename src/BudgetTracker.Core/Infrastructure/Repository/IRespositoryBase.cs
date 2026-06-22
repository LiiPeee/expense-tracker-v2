namespace BudgetTracker.Core.Domain.Repository;

/// <summary>
/// Base contract for repositories of entities that are NOT scoped to an account
/// (reference/lookup data and aggregate roots such as Account).
/// </summary>
public interface IRepositoryBase<T> where T : class
{
    Task<T> AddAsync(T entity);
    Task<T?> GetByIdAsync(long id);
    Task<bool> UpdateAsync(T entity);
    Task<bool> DeleteAsync(long id);
}
