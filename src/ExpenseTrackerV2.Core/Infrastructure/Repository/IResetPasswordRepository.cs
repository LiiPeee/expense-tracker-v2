
using ExpenseTrackerV2.Core.Domain.Entities;
using ExpenseTrackerV2.Core.Domain.Repository;

namespace ExpenseTrackerV2.Core.Infrastructure.Repository
{
    public interface IResetPasswordRepository : IRepositoryBase<ResetPassword>
    {
        Task<ResetPassword?> GetByAccountIdAsync(long accountId);
    }
}
