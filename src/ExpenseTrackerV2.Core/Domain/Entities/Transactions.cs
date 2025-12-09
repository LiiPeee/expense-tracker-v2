

namespace ExpenseTrackerV2.Core.Domain.Entities;

public class Transactions : BaseEntity
{
    public decimal Amount { get; set; }
    public required string Name { get; set; }
    public string Description { get; set; } = null!;
    public bool Paid { get; set; }
    public int? NumberOfInstallment { get; set; }
    public DateOnly? DateOfInstallment { get; set; }
    public long RecurrenceId { get; set; }
    public Contact Contact { get; set; } = null!;
    public long? ContactId { get; set; }
    public long AccountId { get; set; }
    public Account Account { get; set; } = null!;
    public Category Category { get; set; } = null!;
    public long CategoryId { get; set; }
    public long TypeTransactionId { get; set; }
}
