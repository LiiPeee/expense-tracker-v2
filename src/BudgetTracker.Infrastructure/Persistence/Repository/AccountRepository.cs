using Dapper;
using BudgetTracker.Core.Domain.Entities;
using BudgetTracker.Core.Domain.Repository;
using System.Data;

namespace BudgetTracker.Infrastructure.Persistence.Repository;

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

    public async Task UpdateBalanceAtomicAsync(long accountId, decimal delta)
    {
        if (_db._connection.State != ConnectionState.Open) throw new InvalidOperationException("Error of connection");

        const string query = @"UPDATE ""Account"" SET ""Balance"" = ""Balance"" + @Delta WHERE ""Id"" = @AccountId";
        await _db._connection.ExecuteAsync(query, new { Delta = delta, AccountId = accountId }, _db._transaction);
    }
}

