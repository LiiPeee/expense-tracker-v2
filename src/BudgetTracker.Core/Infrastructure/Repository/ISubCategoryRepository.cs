using BudgetTracker.Core.Domain.Entities;
using BudgetTracker.Core.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetTracker.Core.Infrastructure.Repository
{
    public interface ISubCategoryRepository : IRepositoryBase<SubCategory>
    {
        Task<SubCategory?> GetByNameAsync(long accountId, string name, long? categoryId);
    }
}


