using ExpenseTrackerV2.Application.Dtos.Request;
using ExpenseTrackerV2.Application.Service;
using ExpenseTrackerV2.Core.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTrackerV2.WebApi.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController(AccountAppService accountAppService) : ControllerBase
    {
        private readonly AccountAppService _accountAppService = accountAppService;

        [HttpPost("[action]")]
        public async Task<Account> CreateAsync([FromBody] AccountRequest request)
        {
            return await _accountAppService.CreateAsync(request);
        }
    }
}
