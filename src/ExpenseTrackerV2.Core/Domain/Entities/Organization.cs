using System;

namespace ExpenseTrackerV2.Core.Domain.Entities;

public class Organization
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public List<Account> Accounts { get; set; } = new();
}
