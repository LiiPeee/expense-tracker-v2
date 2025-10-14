using System;

namespace ExpenseTrackerV2.Core.Domain.Entities;

public class Address : BaseEntity
{
    public string Street { get; set; } = null!;
    public string City { get; set; } = null!;
    public string State { get; set; } = null!;
    public string ZipCode { get; set; } = null!;
    public string Country { get; set; } = null!;
    public Contact Contact { get; set; } = null!;
    public long ContactId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsPrimary { get; set; }
}
