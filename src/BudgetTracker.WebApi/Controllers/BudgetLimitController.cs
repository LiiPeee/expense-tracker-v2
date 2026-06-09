using System.Security.Claims;
using BudgetTracker.Core.Domain.Entities;
using BudgetTracker.Core.Domain.Service;
using BudgetTracker.WebApi.Mapper;
using BudgetTracker.WebApi.Models.BudgetLimit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BudgetTracker.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "User,Admin")]
    public class BudgetLimitController(IBudgetLimitService budgetLimitService) : ControllerBase
    {
        private readonly IBudgetLimitService _budgetLimitService = budgetLimitService;
        [HttpPost("[action]")]
        public async Task<ActionResult> CreateAsync([FromBody] CreateBudgetLimitRequest request)
        {
            var accountId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            return Ok(await _budgetLimitService.CreateAsync(request.ToCreateBudgetLimit(accountId)));
        }

        [HttpGet("[action]")]
        public async Task<ActionResult> GetByAccountIdAsync( [FromQuery] int pageNumber)
        {
            var accountId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            return Ok(await _budgetLimitService.GetByAccountIdAsync(accountId, pageNumber));
        }
    }
}
