using Dapper;
using BudgetTracker.Core.Domain.Repository;

namespace BudgetTracker.Infrastructure.Persistence.Repository;

/// <summary>
/// Repository base for entities that are NOT account-scoped (reference data and
/// aggregate roots such as Account). Rows are addressed by primary key only.
/// </summary>
public class RepositoryBase<T> : RepositoryRoot<T>, IRepositoryBase<T> where T : class
{
    public RepositoryBase(DbSession session) : base(session)
    {
    }

    public async Task<T?> GetByIdAsync(long id)
    {
        var query = $"SELECT * FROM {_tableName} WHERE id = @Id";

        EnsureConnectionOpen();

        return await _db._connection.QuerySingleOrDefaultAsync<T>(query, new { Id = id }, _db._transaction);
    }

    public async Task<bool> UpdateAsync(T entity)
    {
        var properties = GetProperties(entity, excludeKey: true);
        var setClause = string.Join(", ", properties.Keys.Select(k => $"{k} = @{k}"));

        var query = $"UPDATE {_tableName} SET {setClause} WHERE id = @Id";

        EnsureConnectionOpen();

        var result = await _db._connection.ExecuteAsync(query, entity, _db._transaction);
        return result > 0;
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var query = $"DELETE FROM {_tableName} WHERE id = @Id";

        EnsureConnectionOpen();

        var result = await _db._connection.ExecuteAsync(query, new { Id = id }, _db._transaction);
        return result > 0;
    }
}
