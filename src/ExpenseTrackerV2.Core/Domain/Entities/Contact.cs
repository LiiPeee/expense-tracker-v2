using System;
using ExpenseTrackerV2.Core.Domain.Enums;

namespace ExpenseTrackerV2.Core.Domain.Entities;

public class Contact : BaseEntity
{
    public string Name { get; set; } = null!;
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Document { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public List<Address> Address { get; set; }
    public long? AddressId { get; set; }
    public TypeContact Type { get; set; } = TypeContact.Personal;
    public bool IsActive { get; set; } = true;
    public List<Transactions> Transaction { get; set; } = new();
}
