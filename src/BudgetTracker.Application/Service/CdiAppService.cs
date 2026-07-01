using BudgetTracker.Core.Domain.Service;
using BudgetTracker.Core.Infrastructure.OutPut;
using BudgetTracker.Core.Infrastructure.Services;

namespace BudgetTracker.Application.Service;


public class CdiAppService(IBacenService bacenService) : ICdiAppService
{
    private readonly IBacenService _bacenService = bacenService;
    public async Task<IEnumerable<BacenOutPut>> CdiHistoryAsync(DateOnly from, DateOnly to)
    {

        string bacenFrom = from.ToString("dd/MM/yyyy");
        string bacenTo = to.ToString("dd/MM/yyyy");

        var response = await _bacenService.GetHistoryCdiAsync(bacenFrom, bacenTo);

        return response;
    }
}