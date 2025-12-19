using Dapper;
using ExpenseTrackerV2.Core.Domain.Entities;
using ExpenseTrackerV2.Core.Domain.Repository;
using System.Data;

namespace ExpenseTrackerV2.Infrastructure.Persistence.Repository;

public class CategoryRepository : RepositoryBase<Category>, ICategoryRepository
{
    public CategoryRepository(DbSession connection) : base(connection)
    {
    }

    public async Task<Category?> GetByNameAsync(string name)
    {
        var query = $"SELECT * FROM Category WHERE Name = @Name";

        if (_db._connection.State == ConnectionState.Open)
        {
            return await _db._connection.QuerySingleOrDefaultAsync<Category?>(query, new { Name = name }, transaction: _db._transaction);
        }
        else
        {
            throw new Exception("lost connection");
        }
    }
}
