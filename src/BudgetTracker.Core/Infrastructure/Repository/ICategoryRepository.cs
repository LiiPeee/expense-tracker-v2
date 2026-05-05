using BudgetTracker.Core.Domain.Entities;

namespace BudgetTracker.Core.Domain.Repository;

public interface ICategoryRepository : IRepositoryBase<Category>
{
    Task<Category?> GetByNameAsync(string name);
    Task<List<Category>> GetAllAsync();
}


