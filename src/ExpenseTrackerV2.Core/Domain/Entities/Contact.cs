
using ExpenseTrackerV2.Core.Domain.Enum;

namespace ExpenseTrackerV2.Core.Domain.Entities;

public class Contact : BaseEntity
{
    public string Name { get; set; } = null!;
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Document { get; set; }
    public bool IsActive { get; set; } = true;
    public Account Account { get; set; }
    public long AccountId { get; set; }
    public long TypeContactId { get; set; }
    public List<Transactions>? Transaction { get; set; }
    public List<Address>? Address { get; set; }
}
