
namespace ExpenseTrackerV2.Core.Domain.Models.Request
{
    public class CreateSubCategoryRequest
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
        public long? CategoryId { get; set; }
    }
}
