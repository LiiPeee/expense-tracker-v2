using System;

namespace BudgetTracker.Core.Domain.Entities;

public class Address : BaseEntity, IAccountOwned
{
    public string Street { get; set; } = null!;
    public string City { get; set; } = null!;
    public string State { get; set; } = null!;
    public string ZipCode { get; set; } = null!;
    public string Country { get; set; } = null!;
    public bool IsPrimary { get; set; }
    public long ContactId { get; set; }
    public long AccountId { get; set; }
}


