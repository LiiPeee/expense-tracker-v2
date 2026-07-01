using BudgetTracker.Core.Infrastructure.OutPut;

namespace BudgetTracker.Core.Domain.Service;

public interface ICdiAppService
{
    Task<IEnumerable<BacenOutPut>> CdiHistoryAsync(DateOnly from, DateOnly to);
}
