using Dapper;
using BudgetTracker.Core.Domain.Entities;
using BudgetTracker.Core.Domain.Repository;
using BudgetTracker.Core.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetTracker.Infrastructure.Persistence.Repository
{
    public class SubCategoryRepository : RepositoryBase<SubCategory>, ISubCategoryRepository
    {
        public SubCategoryRepository(DbSession connection) : base(connection)
        {
        }


        public async Task<SubCategory?> GetByNameAsync(long accountId, string name)
        {
            var query = $"SELECT * FROM SubCategory WHERE Name = @Name";

            if (_db._connection.State == ConnectionState.Open)
            {
                return await _db._connection.QuerySingleOrDefaultAsync<SubCategory>(query, new { Name = name }, transaction: _db._transaction);
            }
            else
            {
                throw new Exception("lost connection");
            }
        }
    }
}


