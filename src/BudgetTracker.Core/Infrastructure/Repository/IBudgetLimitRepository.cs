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
    public interface IBudgetLimitRepository : IAccountScopedRepository<BudgetLimit>
    {
        Task<BudgetLimit?> GetByCategoryAndAccountIdAsync(long categoryId, long accountId);
        Task<IPagedResult<BudgetLimit?>> GetByAccountIdAsync(long month, long year,long accountId, int pageNumber = 1);
    }
}
