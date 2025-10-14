using System.Data;
using System.Data.SqlClient;
using ExpenseTrackerV2.Core.Domain.Entities;
using Microsoft.Extensions.Configuration;

namespace ExpenseTrackerV2.Infrastructure.Persistence.Repository;

public class DapperContext
{
    private readonly string _connectionString;

    public DapperContext(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("ExpenseTrackerDbContext");
    }

    public IDbConnection CreateConnection()
    {
        return new SqlConnection(_connectionString);
    }

    public string ConnectionString => _connectionString;
}
