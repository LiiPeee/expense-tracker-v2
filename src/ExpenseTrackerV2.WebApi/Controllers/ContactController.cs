using ExpenseTrackerV2.Application.Dtos.Request;
using ExpenseTrackerV2.Application.Service;
using ExpenseTrackerV2.Core.Domain.Entities;
using ExpenseTrackerV2.Core.Domain.Service;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Roles = "Admin,User")]
        [HttpPost("[action]")]
        public async Task<Contact?> CreateAsync([FromBody] ContactRequest contactRequest)
        {
            return await _contactAppService.CreateAsync(contactRequest);
        }

        [Authorize(Roles = "Admin,User")]
        [HttpGet("[action]")]
        public async Task<List<Contact?>> GetAllAsync()
        {
            return await _contactAppService.GetAllsync();
        }
        [HttpPost("[action]")]
        public async Task EditContactAsync([FromBody] ContactRequest contactRequest)
        {
             await _contactAppService.GetAllsync();
        }

        [Authorize(Roles = "Admin,User")]
        [HttpDelete("[action]")]
        public async Task DeleteContactAsync([FromQuery] string contactId)
        {
            await _contactAppService.DeleteContactAsync(contactId);
        }
    }
}