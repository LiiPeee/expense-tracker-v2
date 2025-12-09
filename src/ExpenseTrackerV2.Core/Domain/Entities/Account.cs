using System;
using System.Text.Json.Serialization;

namespace ExpenseTrackerV2.Core.Domain.Entities;

public class Account : BaseEntity
{
    public long Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; }
    public string Password { get; set; } = null!;
    public decimal Balance { get; set; }
    public bool IsActive { get; set; } = true;
    public long OrganizationId { get; set; }
    public Organization Organization { get; set; }
    public List<Transactions> Transactions { get; set; } = new();
}
