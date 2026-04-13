using ExpenseTrackerV2.Application.Dtos.Request;
using ExpenseTrackerV2.Application.Service;
using ExpenseTrackerV2.Core.Domain.Entities;
using ExpenseTrackerV2.Core.Domain.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ExpenseTrackerV2.WebApi.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "User")]
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
            var accountId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            return await _contactAppService.CreateAsync(accountId, contactRequest);
        }

        [HttpGet("[action]")]
        public async Task<List<Contact?>> GetAllAsync()
        {
            var accountId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            return await _contactAppService.GetAllsync(accountId);
        }
        [HttpPost("[action]")]
        public async Task EditContactAsync([FromBody] ContactRequest contactRequest)
        {
            var accountId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            await _contactAppService.EditContactAsync(accountId, contactRequest);
        }

        [HttpDelete("[action]")]
        public async Task DeleteContactAsync([FromQuery] string contactId)
        {
            var accountId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            await _contactAppService.DeleteContactAsync(accountId, contactId);
        }
    }
}