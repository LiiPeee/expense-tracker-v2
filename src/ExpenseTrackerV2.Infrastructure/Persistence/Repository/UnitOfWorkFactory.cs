

namespace ExpenseTrackerV2.Infrastructure.Persistence.Repository
{
    using ExpenseTrackerV2.Core.Domain.Entities;
    using ExpenseTrackerV2.Core.Domain.UnitOfWork;
    using ExpenseTrackerV2.Infrastructure.UnitOfWork;
    using System.Data;
    using System.Threading.Tasks;

    public class UnitOfWorkFactory<TConnection> : IUnitOfWorkFactory where TConnection : IDbConnection, new()
    {
        private string _connectionString;
    
        public UnitOfWorkFactory(string _connectionString)
        {
            if (string.IsNullOrEmpty(_connectionString))
            {
                throw new ArgumentNullException("connectionString cannot be null");
            }
            this._connectionString = _connectionString;
        }
    
        public IDbTransaction Transaction => throw new NotImplementedException();

        public IUnitOfWork Create()
        {
            return new UnitOfWork(CreateOpenConnection());
        }

        private IDbConnection CreateOpenConnection()
        {
            var conn = new TConnection();
            conn.ConnectionString = _connectionString;

            try
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
            }
            catch (Exception ex) 
            {
                throw new Exception("An Error Occurred while connecting to the database");  
            }

            return conn;
        }
    }
}
