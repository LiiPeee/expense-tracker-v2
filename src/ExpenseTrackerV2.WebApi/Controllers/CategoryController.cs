using ExpenseTrackerV2.Application.Service;
using ExpenseTrackerV2.Core.Domain.Dtos.Request.Transaction;
using ExpenseTrackerV2.Core.Domain.Entities;
using ExpenseTrackerV2.Core.Domain.Models.Output;
using ExpenseTrackerV2.Core.Domain.Service;
using ExpenseTrackerV2.WebApi.Mapping;
using ExpenseTrackerV2.WebApi.Models.Category;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTrackerV2.WebApi.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        public readonly ICategoryAppService _categoryAppService;
        public CategoryController(ICategoryAppService categoryAppService)
        {
            _categoryAppService = categoryAppService;
        }
        [Authorize(Roles = "User")]
        [HttpGet("[action]")]
        public async Task<IEnumerable<AllCategoriesOutPut>> GetAllAsync()
        {
            return await _categoryAppService.GetAllAsync();
        }
    }
}