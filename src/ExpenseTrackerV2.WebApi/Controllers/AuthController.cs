
using ExpenseTrackerV2.Core.Domain.Models.Output;
using ExpenseTrackerV2.Core.Domain.Models.Request.Account;
using ExpenseTrackerV2.Core.Domain.Service;
using ExpenseTrackerV2.WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ExpenseTrackerV2.WebApi.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthenticationAppService accountAppService) : ControllerBase
    {
        private readonly IAuthenticationAppService _accountAppService = accountAppService;

        [HttpPost("[action]")]
        public async Task<ActionResult<CreateAccountDto>> SignUpAsync([FromBody] CreateAccountRequest request)
        {
            return Ok(await _accountAppService.CreateAsync(request));
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<string>> VerifyTokenAsync([FromQuery] string token)
        {
            return Ok(await _accountAppService.VerifyEmailAsync(token));
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<string>> ResetPasswordAsync([FromBody] string token)
        {
            return Ok(await _accountAppService.VerifyEmailAsync(token));
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<string>> EmailVerifycationAsync([FromBody] string email)
        {
            return Ok(await _accountAppService.VerifyEmailAsync(email));
        }
        [HttpPost("[action]")]
        public async Task<ActionResult<TokenResponseDto?>> SignInAsync([FromBody] LoginRequest request)
        {
            var login = await _accountAppService.LoginAsync(request);
            if (login == null)
            {
                return Unauthorized();
            }

            return Ok(login);
        }

        [HttpPost("[action]")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<TokenResponseDto?>> RefreshTokenAsync(RefreshTokenAccountRequestDto request)
        {
            var accountId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var requestMapper = new RefreshTokenRequestDto()
            {
                AccountId = accountId,
                RefreshToken = request.RefreshToken
            };

            var result = await _accountAppService.RefreshTokenAsync(requestMapper);

            return Ok(result);
        }

        [HttpPost("[action]")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<TokenResponseDto?>> LogOutAsync()
        {
            var accountId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var result = await _accountAppService.LogOutAsync(accountId);

            if (result is null)
            {
                return BadRequest();
            }
            return Ok(new { message = result });
        }
    }
}