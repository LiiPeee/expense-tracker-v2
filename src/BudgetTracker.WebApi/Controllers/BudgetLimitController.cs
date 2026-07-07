using BudgetTracker.Core.Domain.Entities;
using BudgetTracker.Core.Domain.Service;
using BudgetTracker.WebApi.Mapper;
using BudgetTracker.WebApi.Models.BudgetLimit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

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
        public async Task<ActionResult> GetByAccountIdAsync([FromQuery][Range(1, 12)] long month,
            [FromQuery][Range(2000, 2100)] long year, [FromQuery] int pageNumber)
        {
            var accountId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            return Ok(await _budgetLimitService.GetByAccountIdAsync(month,year, accountId, pageNumber));
        }
    }
}
