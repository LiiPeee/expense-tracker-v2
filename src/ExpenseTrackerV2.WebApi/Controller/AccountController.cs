using ExpenseTrackerV2.Application.Dtos.Request;
using ExpenseTrackerV2.Application.Service;
using ExpenseTrackerV2.WebApi.Filter;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTrackerV2.WebApi.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController(AccountAppService accountAppService) : ControllerBase
    {
        private readonly AccountAppService _accountAppService = accountAppService;

        [HttpPost("[action]")]
        [AccountRequest]
        public async Task CreateAsync([FromBody] AccountRequest request)
        {
             await _accountAppService.CreateAsync(request);
        }
    }
}
