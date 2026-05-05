using System.Data;

namespace BudgetTracker.Core.Domain.UnitOfWork;

public interface IUnitOfWork
{    
    void BeginTransaction();
    void Commit();
    void Rollback();
}


