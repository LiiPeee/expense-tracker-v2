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
    public class CdiController(ICdiAppService cdiAppService) : ControllerBase
    {
        private readonly ICdiAppService _cdiAppService = cdiAppService;


        [HttpGet("[action]")]
        public async Task<ActionResult> HistoryAsync([FromQuery] DateOnly from, [FromQuery] DateOnly to)
        {
            var response = await _cdiAppService.CdiHistoryAsync(from, to);

            return Ok(response);
        }
    }
}
