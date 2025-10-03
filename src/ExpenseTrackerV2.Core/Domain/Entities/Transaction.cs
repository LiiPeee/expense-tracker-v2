using System;
using System.Diagnostics.Contracts;
using System.Text.Json.Serialization;
using ExpenseTrackerV2.Core.Domain.Enum;

namespace ExpenseTrackerV2.Core.Domain.Entities;

public class Transaction
{
    public long Id { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; } = null!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public Category Category { get; set; } = null!;
    public long CategoryId { get; set; }
    public bool Paid { get; set; }
    public int? NumberOfInstallment { get; set; }
    public DateTime? DateOfInstallment { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Recurrence Recurrence { get; set; } = Recurrence.None;
    public Contact Contact { get; set; } = null!;
    public long? ContactId { get; set; }
    [JsonIgnore]
    public long AccountId { get; set; }
    [JsonIgnore]
    public Account Account { get; set; } = null!;

}
