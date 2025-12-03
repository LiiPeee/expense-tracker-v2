using Dapper;
using ExpenseTrackerV2.Core.Domain.Entities;
using ExpenseTrackerV2.Core.Domain.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;

namespace ExpenseTrackerV2.Infrastructure.Persistence.Repository;

public class CategoryRepository : RepositoryBase<Category>, ICategoryRepository
{
    public CategoryRepository(DapperContext context) : base(context)
    {
    }

    public async Task<Category?> GetByNameAsync(string name)
    {
        var query = $"SELECT * FROM Category WHERE Name = @Name";

        if (_context.CurrentConnection != null)
        {
          return await _context.CurrentConnection.QuerySingleOrDefaultAsync<Category>(query, new { Name = name}, _context.CurrentTransaction);
        }
        else
        {
            throw new Exception("lost connection");
        }
    }
}
