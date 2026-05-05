using BudgetTracker.Core.Domain.Entities;
using BudgetTracker.Core.Domain.Models.Request;

namespace BudgetTracker.Core.Domain.Service
{
    public interface ISubCategoryAppService
    {
        Task CreateAsync(long accountId, CreateSubCategoryRequest request);
        Task<IEnumerable<SubCategory>> GetAllAsync();
    }
}


