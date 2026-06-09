using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BudgetTracker.Core.Domain.Dtos.Output;
using BudgetTracker.Core.Domain.Entities;
using BudgetTracker.Core.Domain.Repository;

namespace BudgetTracker.Core.Infrastructure.Repository
{
    public interface IBudgetLimitRepository : IRepositoryBase<BudgetLimit>
    {
        Task<BudgetLimit?> GetByCategoryAndAccountIdAsync(string categoryName, long accountId);
        Task<IPagedResult<BudgetLimit?>> GetByAccountIdAsync(long accountId, int pageNumber = 1);
    }
}
