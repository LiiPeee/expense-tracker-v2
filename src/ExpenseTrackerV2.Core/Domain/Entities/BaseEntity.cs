using System;

namespace ExpenseTrackerV2.Core.Domain.Entities;

public class BaseEntity : IBaseEntity
{
    public long Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}
