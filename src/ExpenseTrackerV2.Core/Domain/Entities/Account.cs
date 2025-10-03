using System;
using System.Text.Json.Serialization;

namespace ExpenseTrackerV2.Core.Domain.Entities;

public class Account
{
    public long Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string? Email { get; set; }
    public string Password { get; set; } = null!;
    public decimal Balance { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    [JsonIgnore]
    public long OrganizationId { get; set; }
    [JsonIgnore]
    public Organization Organization { get; set; } = null!;
    public List<Transaction> Transactions { get; set; } = new();
}
