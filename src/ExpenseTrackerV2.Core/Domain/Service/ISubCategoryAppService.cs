using ExpenseTrackerV2.Core.Domain.Entities;
using ExpenseTrackerV2.Core.Domain.Models.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTrackerV2.Core.Domain.Service
{
    public interface ISubCategoryAppService
    {
        Task CreateAsync(CreateSubCategoryRequest request);
        Task<IEnumerable<SubCategory>> GetAllAsync();
    }
}
