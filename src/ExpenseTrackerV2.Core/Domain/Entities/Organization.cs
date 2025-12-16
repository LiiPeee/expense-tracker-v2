using System;

namespace ExpenseTrackerV2.Core.Domain.Entities;

public class Organization : BaseEntity
{
    public string Name { get; set; } = null!;
    public List<Account> Account { get; set; } = new();
}
