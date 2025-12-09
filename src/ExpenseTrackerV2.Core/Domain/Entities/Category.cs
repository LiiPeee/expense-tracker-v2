
namespace ExpenseTrackerV2.Core.Domain.Entities;

public class Category : BaseEntity
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
    public List<Transactions> Transaction { get; set; } = new();
}
