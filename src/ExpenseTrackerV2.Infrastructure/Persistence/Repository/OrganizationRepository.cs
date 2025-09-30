using System;
using ExpenseTrackerV2.Core.Ports.Out.Repository;
using ExpenseTrackerV2.Infrastructure.Persistence.Dbcontext;

namespace ExpenseTrackerV2.Infrastructure.Persistence.Repository;

public class OrganizationRepository(ExpenseTrackerDbContext dbContext) : IOrganizationRepository
{
    private readonly ExpenseTrackerDbContext _dbContext = dbContext;

    public Task<string> CreateAsync()
    {
        return Task.FromResult("Organization Created");
    }

}
