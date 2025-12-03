using ExpenseTrackerV2.Application.Dtos.Request;
using ExpenseTrackerV2.Core.Domain.Entities;
using ExpenseTrackerV2.Core.Domain.Repository;
using ExpenseTrackerV2.Core.Domain.Service;

namespace ExpenseTrackerV2.Application.Service;

public class OrganzationAppService : IOrganizationAppService
{

    private readonly IOrganizationRepository _organizationRepository;

    public OrganzationAppService(IOrganizationRepository organizationRepository)
    {
        _organizationRepository = organizationRepository;
    }

    public async Task<Organization> CreateAsync(OrganizationRequest request)
    {
        var organization = new Organization
        {
            Name = request.Name
        };
        var result = await _organizationRepository.AddAsync(organization);

        if (result == null)
        {
            throw new Exception("Failed to create organization");
        }
        return result;
    }
}
