using ExpenseTrackerV2.Application.Dtos.Request;
using ExpenseTrackerV2.Application.Service;
using ExpenseTrackerV2.Core.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTrackerV2.WebApi.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrganizationController(OrganzationAppService organizationAppService) : ControllerBase
    {
        private readonly OrganzationAppService _organizationAppService = organizationAppService;

        [HttpPost("[action]")]
        public async Task<Organization> CreateAsync([FromBody] OrganizationRequest request)
        {
            return await _organizationAppService.CreateAsync(request);
        }

    }
}
