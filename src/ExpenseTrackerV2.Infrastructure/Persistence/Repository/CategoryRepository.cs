using Dapper;
using ExpenseTrackerV2.Core.Domain.Entities;
using ExpenseTrackerV2.Core.Domain.Repository;

namespace ExpenseTrackerV2.Infrastructure.Persistence.Repository;

public class CategoryRepository : RepositoryBase<Category>, ICategoryRepository
{
    public CategoryRepository(DbSession context) : base(context)
    {
    }

    public async Task<Category?> GetByNameAsync(string name)
    {
        var query = $"SELECT * FROM Category WHERE Name = @Name";

        if (_context.CurrentConnection != null)
        {
            return await _context.CurrentConnection.QuerySingleOrDefaultAsync<Category?>(query, new { Name = name }, _context.CurrentTransaction);
        }
        else
        {
            throw new Exception("lost connection");
        }
    }
}
