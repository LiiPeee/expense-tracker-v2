using Dapper;
using BudgetTracker.Core.Domain.Entities;
using BudgetTracker.Core.Domain.Repository;

namespace BudgetTracker.Infrastructure.Persistence.Repository;

/// <summary>
/// Repository base for account-owned entities. Every row-targeting operation is
/// filtered by <c>AccountId</c>, so a caller cannot read, update or delete a row
/// that belongs to another account even when it knows the primary key.
/// </summary>
public class AccountScopedRepositoryBase<T> : RepositoryRoot<T>, IAccountScopedRepository<T>
    where T : class, IAccountOwned
{
    private const int MaxRows = 1000;

    public AccountScopedRepositoryBase(DbSession session) : base(session)
    {
    }

    public async Task<T?> GetByIdAsync(long id, long accountId)
    {
        var query = $"SELECT * FROM {_tableName} WHERE id = @Id AND AccountId = @AccountId";

        EnsureConnectionOpen();

        return await _db._connection.QuerySingleOrDefaultAsync<T>(
            query, new { Id = id, AccountId = accountId }, _db._transaction);
    }

    public async Task<IEnumerable<T>> GetAllAsync(long accountId)
    {
        var query = $"SELECT * FROM {_tableName} WHERE AccountId = @AccountId LIMIT {MaxRows}";

        EnsureConnectionOpen();

        var result = await _db._connection.QueryAsync<T>(
            query, new { AccountId = accountId }, _db._transaction);
        return result.ToList();
    }

    public async Task<bool> UpdateAsync(T entity)
    {
        var properties = GetProperties(entity, excludeKey: true);
        var setClause = string.Join(", ", properties.Keys.Select(k => $"{k} = @{k}"));

        var query = $"UPDATE {_tableName} SET {setClause} WHERE id = @Id AND AccountId = @AccountId";

        EnsureConnectionOpen();

        var result = await _db._connection.ExecuteAsync(query, entity, _db._transaction);
        return result > 0;
    }

    public async Task<bool> DeleteAsync(long id, long accountId)
    {
        var query = $"DELETE FROM {_tableName} WHERE id = @Id AND AccountId = @AccountId";

        EnsureConnectionOpen();

        var result = await _db._connection.ExecuteAsync(
            query, new { Id = id, AccountId = accountId }, _db._transaction);
        return result > 0;
    }
}
