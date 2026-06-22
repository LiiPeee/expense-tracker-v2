using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Reflection;
using Dapper;

namespace BudgetTracker.Infrastructure.Persistence.Repository;

/// <summary>
/// Shared persistence plumbing for Dapper-backed repositories: table-name
/// resolution, property reflection and the insert path. It deliberately exposes
/// no row-targeting reads/writes, so concrete bases decide whether those are
/// scoped by account or not.
/// </summary>
public abstract class RepositoryRoot<T> where T : class
{
    protected readonly DbSession _db;
    protected readonly string _tableName;

    protected RepositoryRoot(DbSession session)
    {
        _db = session;
        _tableName = GetTableName();
    }

    public async Task<T> AddAsync(T entity)
    {
        var properties = GetProperties(entity, excludeKey: true);
        var columns = string.Join(", ", properties.Keys);
        var values = string.Join(", ", properties.Keys.Select(k => $"@{k}"));

        var query = $"INSERT INTO {_tableName} ({columns}) VALUES ({values}) RETURNING id";

        EnsureConnectionOpen();

        var id = await _db._connection.ExecuteScalarAsync<long>(query, entity, _db._transaction);
        var selectQuery = $"SELECT * FROM {_tableName} WHERE id = @Id";
        return await _db._connection.QueryFirstOrDefaultAsync<T>(selectQuery, new { Id = id }, _db._transaction)
            ?? throw new InvalidOperationException($"Failed to retrieve inserted record from {_tableName}.");
    }

    protected void EnsureConnectionOpen()
    {
        if (_db._connection.State != ConnectionState.Open)
            throw new InvalidOperationException("Database connection is not open.");
    }

    protected Dictionary<string, object> GetProperties(T entity, bool excludeKey = false)
    {
        return typeof(T)
            .GetProperties()
            .Where(p => !excludeKey || p.Name != "Id")
            .Where(p => p.GetValue(entity) != null)
            .Where(p => !IsComplexType(p.PropertyType))
            .ToDictionary(p => p.Name, p => p.GetValue(entity)!);
    }

    private static string GetTableName()
    {
        var type = typeof(T);
        var tableAttr = type.GetCustomAttribute<TableAttribute>();
        return tableAttr?.Name ?? type.Name;
    }

    private static bool IsComplexType(Type type)
    {
        if (type.IsGenericType &&
            (type.GetGenericTypeDefinition() == typeof(IEnumerable<>) ||
             type.GetGenericTypeDefinition() == typeof(ICollection<>) ||
             type.GetGenericTypeDefinition() == typeof(List<>)))
        {
            return true;
        }

        if (type.IsArray)
        {
            return true;
        }

        return type.IsClass &&
            type != typeof(string) &&
            type != typeof(DateTime) &&
            type != typeof(decimal);
    }
}
