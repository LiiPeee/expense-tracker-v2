using Npgsql;
using Microsoft.Extensions.Configuration;

using System.Data;


namespace BudgetTracker.Infrastructure.Persistence.Repository
{
    public class DbSession : IDisposable
    {
        public IDbConnection _connection { get; }
        public IDbTransaction? _transaction { get; set; }

        public string _connectrionString { get; set; }

        private bool _disposed;

        public DbSession(IConfiguration configuration)
        {
            _connectrionString = configuration.GetConnectionString("BudgetTracker");
            _connection = new NpgsqlConnection(_connectrionString);
            _connection.Open();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
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


