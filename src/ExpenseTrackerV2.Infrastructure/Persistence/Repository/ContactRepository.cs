using Dapper;
using ExpenseTrackerV2.Core.Domain.Entities;
using ExpenseTrackerV2.Core.Domain.Repository;
using System;

namespace ExpenseTrackerV2.Infrastructure.Persistence.Repository;

public class ContactRepository : RepositoryBase<Contact>, IContactRepository
{
    public ContactRepository(DbSession context) : base(context)
    {
    }

    public async Task<Contact?> GetByNameAsync(string name)
    {
        var query = "SELECT * FROM Contact CC Where Name = @Name";

        if(_context.CurrentConnection is not null)
            return await _context.CurrentConnection.QuerySingleOrDefaultAsync<Contact> (query, new { Name = name }, _context.CurrentTransaction);
        else
        {
            throw new Exception("lost connection");
        }
    }
}
