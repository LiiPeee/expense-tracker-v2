using System;

namespace ExpenseTrackerV2.Core.Domain.Entities;

public class Category
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsActive { get; set; } = true;
    public List<Transaction> Transactions { get; set; } = new();
}
