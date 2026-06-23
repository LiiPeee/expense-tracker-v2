using BudgetTracker.Core.Domain.Dtos.Output;
using BudgetTracker.Core.Domain.Entities;
using BudgetTracker.Core.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetTracker.Core.Infrastructure.Repository
{
    public interface IStockRepository : IAccountScopedRepository<Stock>
    {
        // TODO(Stock WIP): live market-data lookups are part of the unfinished Stock feature.
        public Task<Stock> GetByStockAndAccountAsync(long accountId, string ticker);

        public Task<Stock?> GetByTickerAsync(long accountId, string ticker);
    }
}
