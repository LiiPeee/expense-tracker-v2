using ExpenseTrackerV2.Core.Domain.Entities;
using ExpenseTrackerV2.Core.Domain.Repository;
using System.Data;

namespace ExpenseTrackerV2.Infrastructure.Persistence.Repository;

public class OrganizationRepository : RepositoryBase<Organization>, IOrganizationRepository
{
    public OrganizationRepository(DbSession connection) : base(connection)
    {
    }
}
