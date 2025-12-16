using System.Data;

namespace ExpenseTrackerV2.Core.Domain.UnitOfWork;

public interface IUnitOfWork
{

    IDbTransaction Transaction { get; }

    void Commit();
    void Rollback();
}
