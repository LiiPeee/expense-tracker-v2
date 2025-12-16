using ExpenseTrackerV2.Core.Domain.UnitOfWork;

namespace ExpenseTrackerV2.Infrastructure.UnitOfWork
{
    public interface IUnitOfWorkFactory
    {
        IUnitOfWork Create();

    }
}
