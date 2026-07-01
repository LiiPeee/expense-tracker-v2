using System.Security.Claims;
using BudgetTracker.Core.Domain.Service;
using BudgetTracker.WebApi.Mapper;
using BudgetTracker.WebApi.Models.Stock;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BudgetTracker.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "User,Admin")]
    public class StockController(IStockAppService stockAppService) : ControllerBase
    {
        private readonly IStockAppService _stockAppservice = stockAppService;

        [HttpPost("[action]")]
        public async Task<ActionResult> CreateAsync([FromBody] CreateStock request)
        {
            var accountId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            await _stockAppservice.CreateAsync(accountId, request.ToCreateStock());

            return Ok();
        }

        [HttpGet("[action]")]
        public async Task<ActionResult> GetAllStockAsync([FromQuery] int page = 1)
        {
            var accountId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var response = await _stockAppservice.GetAllStockAsync(accountId, page);

            return Ok(response);
        }

        [HttpGet("[action]")]
        public async Task<ActionResult> GetAllFundsAsync([FromQuery] int page = 1)
        {
            var accountId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var response = await _stockAppservice.GetAllFundsAsync(accountId, page);

            return Ok(response);
        }
    }
}
