using ExpenseTrackerV2.Application.Dtos.Request;
using ExpenseTrackerV2.WebApi.Models.Category;

namespace ExpenseTrackerV2.WebApi.Mapping
{
    internal static class CategoryInputMapping
    {
        internal static CategoryRequest ToCreateCategoryRequest(this CreateCategoryInput input)
        {
            return new CategoryRequest()
            {
                Name = input.Name,
                Description = input.Description
            };
        }
    }
}
