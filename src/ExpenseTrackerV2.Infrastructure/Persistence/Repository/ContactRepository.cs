using Dapper;
using ExpenseTrackerV2.Core.Domain.Entities;
using ExpenseTrackerV2.Core.Domain.Repository;
using System;
using System.Data;

namespace ExpenseTrackerV2.Infrastructure.Persistence.Repository;

public class ContactRepository : RepositoryBase<Contact>, IContactRepository
{
    public ContactRepository(DbSession connection) : base(connection)
    {
    }

    public async Task<Contact?> GetByNameAsync(string name)
    {
        var query = "SELECT * FROM Contact CC Where Name = @Name";

        if(_db._connection.State == ConnectionState.Open)
            return await _db._connection.QuerySingleOrDefaultAsync<Contact> (query, new { Name = name }, transaction: _db._transaction);
        else
        {
            throw new Exception("lost connection");
        }
    }
    
    public async Task<List<Contact?>> GetByIdAccount(long accountId)
    {
        var query = @"SELECT * FROM Contact cc Where AccountId = @AccountId";

        if(_db._connection.State == ConnectionState.Open)
            return (await _db._connection.QueryAsync<Contact> (query, new { AccountId = accountId }, transaction: _db._transaction)).ToList();
        else
        {
            throw new Exception("lost connection");
        }
    }
}
