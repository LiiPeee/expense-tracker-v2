using ExpenseTrackerV2.Core.Domain.Entities;
using ExpenseTrackerV2.Core.Domain.Models.Request;

namespace ExpenseTrackerV2.Core.Domain.Service
{
    public interface ISubCategoryAppService
    {
        Task CreateAsync(long accountId, CreateSubCategoryRequest request);
        Task<IEnumerable<SubCategory>> GetAllAsync();
    }
}
