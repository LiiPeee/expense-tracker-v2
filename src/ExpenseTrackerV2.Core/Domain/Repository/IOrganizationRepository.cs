using System;
using ExpenseTrackerV2.Core.Domain.Entities;

namespace ExpenseTrackerV2.Core.Domain.Repository;

public interface IOrganizationRepository : IRepositoryBase<Organization>
{
    Task<Organization?> AddAsync(Guid id);
}
