using BudgetTracker.Core.Domain.Entities;
using BudgetTracker.Core.Domain.Repository;
using BudgetTracker.Core.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetTracker.Infrastructure.Persistence.Repository
{
    public class StockRepository : AccountScopedRepositoryBase<Stock>, IStockRepository
    {
        public StockRepository(DbSession session) : base(session)
        {
        }

        // TODO(Stock WIP): live market-data lookups; stubbed to satisfy IStockRepository.
        public Task<Stock> GetByStockAndAccountAsync(long accountId, string ticker)
            => throw new NotImplementedException("Stock feature WIP.");

        public Task<Stock?> GetByTickerAsync(long accountId, string ticker)
            => throw new NotImplementedException("Stock feature WIP.");
    }
}
