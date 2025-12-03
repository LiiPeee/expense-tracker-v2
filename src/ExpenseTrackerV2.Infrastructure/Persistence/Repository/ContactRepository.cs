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

    public async Task<long Id> GetByNameAsync(string name)
    {
        var query = "SELECT cc.Id FROM Contact Where Name = @Name SELECT Contact.Id";
        if(_context.CurrentConnection is not null)
            return _context.CurrentConnection.QuerySingleOrDefaultAsync<Contact>()
    }
}
