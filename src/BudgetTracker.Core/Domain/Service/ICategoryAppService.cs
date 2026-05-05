using BudgetTracker.Application.Dtos.Request;
using BudgetTracker.Core.Domain.Models.Output;


namespace BudgetTracker.Core.Domain.Service
{
    public interface ICategoryAppService
    {
        Task CreateAsync(CategoryRequest request);

        Task<IEnumerable<AllCategoriesOutPut>> GetAllAsync();
    }
}


