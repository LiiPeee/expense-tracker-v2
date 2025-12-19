using ExpenseTrackerV2.Core.Domain.Repository;
using ExpenseTrackerV2.Core.Domain.UnitOfWork;
using ExpenseTrackerV2.Core.Infrastructure.Repository;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ExpenseTrackerV2.Infrastructure.Persistence.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbSession _session;

        public UnitOfWork(DbSession session)
        {
            _session = session;
        }

        public void BeginTransaction()
        {
            _session._transaction = _session._connection.BeginTransaction();
        }

        public void Commit()
        {
            try
            {
                _session._transaction.Commit();
                _session._transaction.Connection.Close();
            }
            catch (Exception)
            {
                _session._transaction.Rollback();
                _session._transaction.Dispose();

                throw;
            }
            finally
            {
                _session._transaction?.Dispose();
            }
        }
      
        public void Rollback()
        {
            try
            {
                _session._transaction.Rollback();
            }
          
            finally
            {
                _session._transaction?.Dispose();
            }
        }
        public void Dispose()
        {
            _session.Dispose();
        }
    }
}
