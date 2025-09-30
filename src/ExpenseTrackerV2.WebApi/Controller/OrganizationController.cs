using ExpenseTrackerV2.Application.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTrackerV2.WebApi.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrganizationController(OrganzationAppService organizationAppService) : ControllerBase
    {
        private readonly OrganzationAppService _organizationAppService = organizationAppService;

        [HttpPost("[action]")]
        public Task<string> CreateAsync()
        {
            return Task.FromResult("Organization Created");
        }

    }
}
