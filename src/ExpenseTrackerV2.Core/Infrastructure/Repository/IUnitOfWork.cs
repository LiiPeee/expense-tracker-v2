using System.Data;

namespace ExpenseTrackerV2.Core.Domain.UnitOfWork;

public interface IUnitOfWork
{    
    void BeginTransaction();
    void Commit();
    void Rollback();
}
