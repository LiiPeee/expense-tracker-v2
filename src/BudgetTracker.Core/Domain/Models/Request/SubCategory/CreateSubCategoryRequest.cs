namespace BudgetTracker.Core.Domain.Models.Request.SubCategory
{
    public class CreateSubCategoryRequest
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
        public long? CategoryId { get; set; }
    }
}


