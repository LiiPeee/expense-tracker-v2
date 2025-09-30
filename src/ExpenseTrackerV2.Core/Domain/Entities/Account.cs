using System;
using System.Text.Json.Serialization;

namespace ExpenseTrackerV2.Core.Domain.Entities;

public class Account
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public decimal Balance { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    [JsonIgnore]
    public long OrganizationId { get; set; }
    [JsonIgnore]
    public Organization Organization { get; set; } = null!;
    public List<Transaction> Transactions { get; set; } = new();
}
