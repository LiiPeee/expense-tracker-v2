
using ExpenseTrackerV2.Core.Domain.Models.Output;
using ExpenseTrackerV2.Core.Domain.Service;

using Microsoft.AspNetCore.Authorization;
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