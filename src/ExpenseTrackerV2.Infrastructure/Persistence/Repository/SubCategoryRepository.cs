using Dapper;
using ExpenseTrackerV2.Core.Domain.Entities;
using ExpenseTrackerV2.Core.Domain.Repository;
using ExpenseTrackerV2.Core.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTrackerV2.Infrastructure.Persistence.Repository
{
    public class SubCategoryRepository : RepositoryBase<SubCategory>, ISubCategoryRepository
    {
        public SubCategoryRepository(DbSession context) : base(context)
        {
        }


        public async Task<SubCategory?> GetByNameAsync(string name)
        {
            var query = $"SELECT * FROM Category WHERE Name = @Name";

            if (_context.CurrentConnection != null)
            {
                return await _context.CurrentConnection.QuerySingleOrDefaultAsync<SubCategory>(query, new { Name = name }, _context.CurrentTransaction);
            }
            else
            {
                throw new Exception("lost connection");
            }
        }
    }
}
