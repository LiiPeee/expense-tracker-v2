using System;
using ExpenseTrackerV2.Core.Domain.Entities;
using ExpenseTrackerV2.Core.Domain.Repository;

namespace ExpenseTrackerV2.Infrastructure.Persistence.Repository;

public class CategoryRepository : RepositoryBase<Category>, ICategoryRepository
{
    public CategoryRepository(DapperContext context) : base(context)
    {
    }
}
