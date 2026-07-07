using BudgetTracker.Core.Domain.Dtos.Output;
using BudgetTracker.Core.Domain.Entities;
using BudgetTracker.Core.Domain.Models.Output;
using BudgetTracker.Core.Domain.Models.Request.BudgetLimit;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetTracker.Core.Domain.Service
{
    public interface IBudgetLimitService
    {
        Task<BudgetLimit> CreateAsync(CreateBudgetLimit request);
        Task<IPagedResult<BudgetLimitOutput>> GetByAccountIdAsync(long month,long year, long accountId, int pageNumber = 1);
    }
}
