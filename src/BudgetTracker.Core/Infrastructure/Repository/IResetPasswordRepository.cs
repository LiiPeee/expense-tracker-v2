
using BudgetTracker.Core.Domain.Entities;
using BudgetTracker.Core.Domain.Repository;

namespace BudgetTracker.Core.Infrastructure.Repository
{
    public interface IResetPasswordRepository : IRepositoryBase<ResetPassword>
    {
        Task<ResetPassword?> GetByAccountIdAsync(long accountId);
    }
}


