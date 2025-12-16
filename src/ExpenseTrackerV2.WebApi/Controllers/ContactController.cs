using ExpenseTrackerV2.Application.Dtos.Request;
using ExpenseTrackerV2.Application.Service;
using ExpenseTrackerV2.Core.Domain.Entities;
using ExpenseTrackerV2.Core.Domain.Service;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTrackerV2.WebApi.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly IContactAppService _contactAppService;
        public ContactController(IContactAppService contactappService)
        {
            _contactAppService = contactappService;
        }

        [HttpPost("[action]")]
        public async Task<Contact?> CreateAsync([FromBody] ContactRequest contactRequest)
        {
            return await _contactAppService.CreateAsync(contactRequest);
        }
    }
}