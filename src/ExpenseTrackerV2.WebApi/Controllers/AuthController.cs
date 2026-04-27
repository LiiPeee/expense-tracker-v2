
using ExpenseTrackerV2.Core.Domain.Models.Output;
using ExpenseTrackerV2.Core.Domain.Models.Request.Account;
using ExpenseTrackerV2.Core.Domain.Service;
using ExpenseTrackerV2.WebApi.Models.Auth;
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
        public async Task<ActionResult<CreateAccountDto>> SignUpAsync([FromBody] CreateAccountRequestDto request)
        {
            return Ok(await _accountAppService.SignUpAsync(request));
        }
        [HttpPost("[action]")]
        public async Task<ActionResult<TokenResponseDto?>> SignInAsync([FromBody] LoginRequestDto request)
        {
            var login = await _accountAppService.SignInAsync(request);
            if (login == null)
            {
                return Unauthorized();
            }

            return Ok(login);
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<string>> VerifyTokenAsync([FromQuery] VerifyTokenRequest request)
        {
            var requestDto = new VerifyTokenRequestDto()
            {
                Email = request.Email,
                Token = request.Token
            };

            return Ok(await _accountAppService.VerifyTokenAsync(requestDto));
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<string>> ResetPasswordAsync([FromBody] ResetPasswordRequest request)
        {
            var resetPassword = new ResetPasswordRequestDto()
            {
                Token = request.Token,
                NewPassword = request.NewPassword
            };

            return Ok(await _accountAppService.ResetPasswordAsync(resetPassword));
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<string>> EmailVerifycationAsync([FromBody] string email)
        {
            return Ok(await _accountAppService.VerifyEmailAsync(email));
        }
        
        [HttpPost("[action]")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<TokenResponseDto?>> RefreshTokenAsync(RefreshTokenAccountRequest request)
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