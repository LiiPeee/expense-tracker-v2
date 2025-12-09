using System;

namespace ExpenseTrackerV2.Core.Domain.Entities;

public class Organization : BaseEntity
{
    public string Name { get; set; } = null!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public List<Account> Account { get; set; } = new();
}
