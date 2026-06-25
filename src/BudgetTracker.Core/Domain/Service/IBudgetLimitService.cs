using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BudgetTracker.Core.Domain.Dtos.Output;
using BudgetTracker.Core.Domain.Entities;
using BudgetTracker.Core.Domain.Models.Output;
using BudgetTracker.Core.Domain.Models.Request.BudgetLimit;

namespace BudgetTracker.Core.Domain.Service
{
    public interface IBudgetLimitService
    {
        Task<BudgetLimit> CreateAsync(CreateBudgetLimit request);
        Task<IPagedResult<BudgetLimitOutput>> GetByAccountIdAsync(long accountId, int pageNumber = 1);
    }
}
