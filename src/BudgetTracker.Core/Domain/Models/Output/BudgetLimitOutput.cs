namespace BudgetTracker.Core.Domain.Models.Output;

public record BudgetLimitOutput(
    long Id,
    long CategoryId,
    string CategoryName,
    int Month,
    int Year,
    decimal LimitAmount,
    decimal SpentAmount,
    decimal Percentage,
    bool IsLimit
);
