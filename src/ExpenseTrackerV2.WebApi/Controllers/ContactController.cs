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
    }
}