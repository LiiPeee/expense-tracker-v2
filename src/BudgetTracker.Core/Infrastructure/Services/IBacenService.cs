using BudgetTracker.Core.Infrastructure.OutPut;

namespace BudgetTracker.Core.Infrastructure.Services;


public interface IBacenService
{
    Task<IEnumerable<BacenOutPut>> GetHistoryCdiAsync(string from, string to);
}