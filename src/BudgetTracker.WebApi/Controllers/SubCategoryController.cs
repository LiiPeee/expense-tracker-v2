using System.Security.Claims;
using BudgetTracker.Core.Domain.Entities;
using BudgetTracker.Core.Domain.Models.Request.SubCategory;
using BudgetTracker.Core.Domain.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BudgetTracker.WebApi.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,User")]
    public class SubCategoryController : ControllerBase
    {
        private readonly ISubCategoryAppService _subCategoryAppService;
        public SubCategoryController(ISubCategoryAppService subCategoryAppService)
        {
            _subCategoryAppService = subCategoryAppService;
        }
        [HttpPost("[action]")]
        public async Task CreateAsync([FromBody] CreateSubCategoryRequest request)
        {
            var accountId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            await _subCategoryAppService.CreateAsync(accountId, request);
        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<SubCategory>> GetAllAsync()
        {
            var accountId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            return await _subCategoryAppService.GetAllAsync(accountId);
        }
    }
}

