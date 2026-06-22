
using BudgetTracker.Core.Domain.Entities;
using BudgetTracker.Core.Domain.Repository;

namespace BudgetTracker.Core.Infrastructure.Repository
{
    public interface IResetPasswordRepository : IAccountScopedRepository<ResetPassword>
    {
        Task<ResetPassword?> GetByAccountIdAsync(long accountId);
    }
}


