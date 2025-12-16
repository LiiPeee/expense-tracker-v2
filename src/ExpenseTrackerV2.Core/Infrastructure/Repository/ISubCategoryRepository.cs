using ExpenseTrackerV2.Core.Domain.Entities;
using ExpenseTrackerV2.Core.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTrackerV2.Core.Infrastructure.Repository
{
    public interface ISubCategoryRepository : IRepositoryBase<SubCategory>
    {
        Task<SubCategory?> GetByNameAsync(string name);
    }
}
