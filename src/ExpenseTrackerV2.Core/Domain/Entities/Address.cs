using System;

namespace ExpenseTrackerV2.Core.Domain.Entities;

public class Address : BaseEntity
{
    public string Street { get; set; } = null!;
    public string City { get; set; } = null!;
    public string State { get; set; } = null!;
    public string ZipCode { get; set; } = null!;
    public string Country { get; set; } = null!;
    public bool IsPrimary { get; set; }
}
