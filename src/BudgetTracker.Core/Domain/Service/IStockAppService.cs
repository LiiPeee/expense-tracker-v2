using BudgetTracker.Core.Domain.Dtos.Output;
using BudgetTracker.Core.Domain.Entities;
using BudgetTracker.Core.Domain.Models.Output;
using BudgetTracker.Core.Domain.Models.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetTracker.Core.Domain.Service
{
    public interface IStockAppService
    {
        public Task CreateAsync(long accountId, CreateStockRequest request);

        public Task<IPagedResult<GetAllStockResponse>> GetAllStockAsync(long accountId, int page = 1);

    }
}
