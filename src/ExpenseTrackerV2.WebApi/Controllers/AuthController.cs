using ExpenseTrackerV2.Application.Service;
using ExpenseTrackerV2.Core.Domain.Entities;
using ExpenseTrackerV2.Core.Domain.Models.Output;
using ExpenseTrackerV2.Core.Domain.Models.Request.Account;
using ExpenseTrackerV2.Core.Domain.Service;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTrackerV2.WebApi.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthenticationAppService accountAppService) : ControllerBase
    {
        private readonly IAuthenticationAppService _accountAppService = accountAppService;

        [HttpPost("[action]")]
        public async Task<ActionResult<Account>> SignUpAsync([FromBody] CreateAccountRequest request)
        {
            return Ok(await _accountAppService.CreateAsync(request));
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<TokenResponseDto?>> SignInAsync([FromBody] LoginRequest request)
        {
            var login = await _accountAppService.LoginAsync(request);
            if(login == null)
            {
                return Unauthorized();
            }

            return Ok(login);
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<TokenResponseDto?>> RefreshTokenAsync(RefreshTokenRequestDto request)
        {
            var result = await _accountAppService.RefreshTokenAsync(request);

            if(result is null || result.AccessToken is null || result.RefreshToken is null)
            {
                return Unauthorized();
            }
            return Ok(result);
        }
    }
}