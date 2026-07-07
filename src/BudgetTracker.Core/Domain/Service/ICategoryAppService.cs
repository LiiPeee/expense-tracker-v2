using BudgetTracker.Core.Domain.Models.Output;
using BudgetTracker.Core.Domain.Models.Request.Category;


namespace BudgetTracker.Core.Domain.Service
{
    public interface ICategoryAppService
    {
        Task CreateAsync(CategoryRequest request);

        Task<IEnumerable<AllCategoriesOutPut>> GetAllAsync();
    }
}


