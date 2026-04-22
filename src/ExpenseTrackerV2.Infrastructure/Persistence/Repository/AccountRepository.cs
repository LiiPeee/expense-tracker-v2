using Dapper;
using ExpenseTrackerV2.Core.Domain.Entities;
using ExpenseTrackerV2.Core.Domain.Repository;
using System.Data;

namespace ExpenseTrackerV2.Infrastructure.Persistence.Repository;

public class AccountRepository : RepositoryBase<Account>, IAccountRepository
{
    public AccountRepository(DbSession connection) : base(connection)
    {
    }
    public async Task<Account?> GetByEmailAsync(string email)
    {
        var query = @"SELECT * FROM Account WHERE Email = @Email";

        if (_db._connection.State != ConnectionState.Open)
        {
            throw new Exception("connection lost");
        }

        var acount = await _db._connection.QueryFirstOrDefaultAsync<Account>(query, new { Email = email });
        return acount;
    }

    public async Task<Account?> GetByToken(string token)
    {
        var query = @"SELECT * FROM Account WHERE EmailVerificationToken = @EmailVerificationToken";

        var parameters = new DynamicParameters();

        if (_db._connection.State != ConnectionState.Open) throw new InvalidOperationException("Error of connection");

        var account = await _db._connection.QueryFirstOrDefaultAsync<Account>(query, new { EmailVerificationToken = token });

        return account;
    }
}