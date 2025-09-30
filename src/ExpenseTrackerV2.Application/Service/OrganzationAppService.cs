using System;
using ExpenseTrackerV2.Core.Ports.Out.Repository;

namespace ExpenseTrackerV2.Application.Service;

public class OrganzationAppService(IOrganizationRepository organizationRepository)
{
    private readonly IOrganizationRepository _organizationRepository = organizationRepository;

    public Task<string> CreateAsync()
    {
        return Task.FromResult("Organization Created");
    }
}
