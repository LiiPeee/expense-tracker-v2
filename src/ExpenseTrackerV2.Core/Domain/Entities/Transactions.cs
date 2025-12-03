using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ExpenseTrackerV2.Core.Domain.Enum;
using ExpenseTrackerV2.Core.Domain.Enums;

namespace ExpenseTrackerV2.Core.Domain.Entities;

public class Transactions : BaseEntity
{
    public decimal Amount { get; set; }
    public string Description { get; set; } = null!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public Category Category { get; set; } = null!;
    public long CategoryId { get; set; }
    public TypeTransaction TypeTransaction { get; set; } = TypeTransaction.Expense;
    public bool Paid { get; set; }
    public int? NumberOfInstallment { get; set; }
    public DateOnly? DateOfInstallment { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Recurrence Recurrence { get; set; } = Recurrence.None;
    public Contact Contact { get; set; } = null!;
    public long? ContactId { get; set; }
    [JsonIgnore]
    public long AccountId { get; set; }
    [JsonIgnore]
    public Account Account { get; set; } = null!;
}
