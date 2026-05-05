using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

using System.Data;


namespace BudgetTracker.Infrastructure.Persistence.Repository
{
    public class DbSession : IDisposable
    {
        public IDbConnection _connection { get; }
        public IDbTransaction _transaction { get; set; }

        public string _connectrionString { get; set; }

        private bool _disposed;

        public DbSession(IConfiguration configuration)
        {
            _connectrionString = configuration.GetConnectionString("BudgetTracker");
            _connection = new SqlConnection(_connectrionString);
            _connection.Open();
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            if (_connection.State == ConnectionState.Open)
            {
                _connection.Close();
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _transaction?.Dispose();
                    _connection.Dispose();
                }
                _disposed = true;
            }
        }
    }
}


