using BudgetTracker.Core.Infrastructure.OutPut;

namespace BudgetTracker.Core.Infrastructure.Services
{
    public interface IStockMarketService
    {
        Task<List<StockMarketResponse>> GetStockByTickerAsync(List<string> ticker);
    }
}
