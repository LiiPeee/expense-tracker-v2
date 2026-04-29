using System.Security.Claims;
using ExpenseTrackerV2.Application.Dtos.Request;
using ExpenseTrackerV2.Application.Service;
using ExpenseTrackerV2.Core.Domain.Entities;
using ExpenseTrackerV2.Core.Domain.Models.Request;
using ExpenseTrackerV2.Core.Domain.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTrackerV2.WebApi.Controller
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
            return await _subCategoryAppService.GetAllAsync();
        }
    }
}