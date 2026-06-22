using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetTracker.Core.Infrastructure.Services
{
    public interface IStockMarketService
    {
        public Task<string> GetStockByTickerAsync(List<string> ticker);
    }
}
