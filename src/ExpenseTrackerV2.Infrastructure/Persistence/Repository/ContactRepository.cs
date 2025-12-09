using Dapper;
using ExpenseTrackerV2.Core.Domain.Entities;
using ExpenseTrackerV2.Core.Domain.Repository;
using System;

namespace ExpenseTrackerV2.Infrastructure.Persistence.Repository;

public class ContactRepository : RepositoryBase<Contact>, IContactRepository
{
    public ContactRepository(DapperContext context) : base(context)
    {
    }

    public async Task<long> GetByNameAsync(string name)
    {
        var query = "SELECT CC.Id FROM Contact CC Where Name = @Name SELECT Contact.Id";
        if(_context.CurrentConnection is not null)
            return await _context.CurrentConnection.QuerySingleOrDefaultAsync<long> (query, new { Name = name }, _context.CurrentTransaction);
        else
        {
            throw new Exception("lost connection");
        }
    }
}
