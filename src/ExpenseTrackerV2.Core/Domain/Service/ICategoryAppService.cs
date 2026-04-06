using ExpenseTrackerV2.Application.Dtos.Request;
using ExpenseTrackerV2.Core.Domain.Models.Output;


namespace ExpenseTrackerV2.Core.Domain.Service
{
    public interface ICategoryAppService
    {
        Task CreateAsync(CategoryRequest request);

        Task<IEnumerable<AllCategoriesOutPut>> GetAllAsync();
    }
}
