using ExpenseTrackerV2.Core.Domain.UnitOfWork;
using System.Data;

namespace ExpenseTrackerV2.Infrastructure.Persistence.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        public IDbTransaction transaction;

        public UnitOfWork(IDbConnection connection)
        {
            transaction = connection.BeginTransaction();
        }

        public IDbTransaction Transaction => transaction;

        public void Commit()
        {
            try
            {
                transaction.Commit();
                transaction.Connection.Close();
            }
            catch (Exception)
            {
                transaction.Rollback();

                throw;
            }
            finally
            {
                transaction?.Dispose();
                transaction.Connection?.Dispose();
                transaction = null;
            }

        }

        public void Rollback()
        {
            try
            {
                transaction.Rollback();
                transaction.Connection?.Close();
            }
            catch
            {
                throw;
            }
            finally
            {
                transaction?.Dispose();
                transaction.Connection?.Dispose();
                transaction = null;
            }
        }
    }
}
