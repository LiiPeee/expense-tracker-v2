
using System.Data;
using Dapper;
using ExpenseTrackerV2.Core.Domain.Entities;
using ExpenseTrackerV2.Core.Domain.Repository;
using ExpenseTrackerV2.Core.Infrastructure.Repository;

namespace ExpenseTrackerV2.Infrastructure.Persistence.Repository
{
    public class ResetPasswordRepository : RepositoryBase<ResetPassword>, IResetPasswordRepository
    {
        public ResetPasswordRepository(DbSession connection) : base(connection)
        {
        }

       public async Task<ResetPassword?> GetByAccountIdAsync(long accountId)
        {
            var query = @"SELECT * FROM ResetPassword WHERE AccountId = @AccountId";

            var parameters = new { AccountId = accountId };

            if (_db._connection.State != ConnectionState.Open)
            {
                throw new Exception("connection lost");
            }

            var result = await _db._connection.QueryFirstOrDefaultAsync<ResetPassword>(query, parameters);

            return result;
        }
    }
}
