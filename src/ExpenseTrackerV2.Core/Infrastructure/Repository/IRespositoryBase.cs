using System;

namespace ExpenseTrackerV2.Core.Domain.Repository;

public interface IRepositoryBase<T> where T : class
{
    Task<T> AddAsync(T entity);
    Task<T?> GetByIdAsync(long id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<bool> UpdateAsync(T entity);
    Task<bool> DeleteAsync(long id);
}
