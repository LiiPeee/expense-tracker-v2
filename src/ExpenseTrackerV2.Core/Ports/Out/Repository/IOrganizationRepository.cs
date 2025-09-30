using System;

namespace ExpenseTrackerV2.Core.Ports.Out.Repository;

public interface IOrganizationRepository
{
    Task<string> CreateAsync();
}
