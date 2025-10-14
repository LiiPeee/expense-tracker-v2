using System;
using ExpenseTrackerV2.Core.Domain.Repository;

namespace ExpenseTrackerV2.Core.Domain.UnitOfWork;

public interface IUnitOfWork : IDisposable
{
    IOrganizationRepository Organizations { get; }

    int SaveChanges();
}
