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
        public Task<Stock?> GetByStockAndAccountAsync(long accountId, string ticker);
    }
}
