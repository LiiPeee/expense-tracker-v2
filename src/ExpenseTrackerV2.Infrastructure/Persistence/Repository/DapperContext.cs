using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using ExpenseTrackerV2.Core.Domain.Entities;
using Microsoft.Extensions.Configuration;

namespace ExpenseTrackerV2.Infrastructure.Persistence.Repository;

public class DapperContext
{
    private readonly string _connectionString;
    private IDbConnection? _connection;
    private IDbTransaction? _transaction;

    public DapperContext(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("ConnectionStrings__DefaultConnection");
    }

    public IDbConnection CreateConnection()
    {
        return new SqlConnection(_connectionString);
    }

    public string ConnectionString => _connectionString;

    public IDbConnection? CurrentConnection => _connection;
    public IDbTransaction? CurrentTransaction => _transaction;

    public async Task BeginTransactionAsync()
    {
        _connection = CreateConnection();
        if (_connection is DbConnection dbConn)
        {
            await dbConn.OpenAsync();
        }
        else
        {
            _connection.Open();
        }

        _transaction = _connection.BeginTransaction();
    }

    public void CommitTransaction()
    {
        _transaction?.Commit();
        DisposeTransactionAndConnection();
    }

    public void RollbackTransaction()
    {
        try
        {
            _transaction?.Rollback();
        }
        finally
        {
            DisposeTransactionAndConnection();
        }
    }

    private void DisposeTransactionAndConnection()
    {
        _transaction?.Dispose();
        _transaction = null;

        if (_connection != null)
        {
            try
            {
                _connection.Close();
            }
            catch { /* ignore */ }

            _connection.Dispose();
            _connection = null;
        }
    }
}
