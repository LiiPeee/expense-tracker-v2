using ExpenseTrackerV2.Core.Domain.Entities;

namespace ExpenseTrackerV2.Core.Domain.Repository;

public interface ICategoryRepository : IRepositoryBase<Category>
{
    Task<Category?> GetByNameAsync(string name);
}
