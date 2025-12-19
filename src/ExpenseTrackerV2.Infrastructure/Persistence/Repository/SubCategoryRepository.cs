using Dapper;
using ExpenseTrackerV2.Core.Domain.Entities;
using ExpenseTrackerV2.Core.Domain.Repository;
using ExpenseTrackerV2.Core.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTrackerV2.Infrastructure.Persistence.Repository
{
    public class SubCategoryRepository : RepositoryBase<SubCategory>, ISubCategoryRepository
    {
        public SubCategoryRepository(DbSession connection) : base(connection)
        {
        }


        public async Task<SubCategory?> GetByNameAsync(string name)
        {
            var query = $"SELECT * FROM Category WHERE Name = @Name";

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
