using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Reflection;
using Dapper;
using BudgetTracker.Core.Domain.Repository;

namespace BudgetTracker.Infrastructure.Persistence.Repository;

public class RepositoryBase<T> : IRepositoryBase<T> where T : class
{
    protected readonly DbSession _db
        ;

    protected readonly string _tableName;
    public RepositoryBase(DbSession session)
    {
        _db = session;
        _tableName = GetTableName();
    }
    private string GetTableName()
    {
        var type = typeof(T);
        var tableAttr = type.GetCustomAttribute<TableAttribute>();
        return tableAttr?.Name ?? type.Name;
    }

    public async Task<T> AddAsync(T entity)
    {
        var properties = GetProperties(entity, excludeKey: true);
        var columns = string.Join(", ", properties.Keys);
        var values = string.Join(", ", properties.Keys.Select(k => $"@{k}"));

        var query = $"INSERT INTO {_tableName} ({columns}) VALUES ({values}) RETURNING id";

        if (_db._connection.State != ConnectionState.Open)
            throw new InvalidOperationException("Database connection is not open.");

        var id = await _db._connection.ExecuteScalarAsync<long>(query, entity, _db._transaction);
        var selectQuery = $"SELECT * FROM {_tableName} WHERE id = @Id";
        return await _db._connection.QueryFirstOrDefaultAsync<T>(selectQuery, new { Id = id }, _db._transaction)
            ?? throw new InvalidOperationException($"Failed to retrieve inserted record from {_tableName}.");
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        var query = $"SELECT * FROM {_tableName}";

        if (_db._connection.State != ConnectionState.Open)
            throw new InvalidOperationException("Database connection is not open.");

        var result = await _db._connection.QueryAsync<T>(query, transaction: _db._transaction);
        return result.ToList();
    }

    public async Task<T?> GetByIdAsync(long id)
    {
        var query = $"SELECT * FROM {_tableName} WHERE id = @Id";

        if (_db._connection.State == ConnectionState.Open)
        {
            return await _db._connection.QuerySingleOrDefaultAsync<T>(query, new { Id = id }, _db._transaction);
        }
        else
        {
            throw new Exception("connection lost");
        }
    }
    public async Task<bool> UpdateAsync(T entity)
    {
        var properties = GetProperties(entity, excludeKey: true);
        var setClause = string.Join(", ", properties.Keys.Select(k => $"{k} = @{k}"));

        var query = $"UPDATE {_tableName} SET {string.Join(", ", setClause)} WHERE id = @Id";

        if (_db._connection.State == ConnectionState.Open)
        {
            var result = await _db._connection.ExecuteAsync(query, entity, _db._transaction);
            return result > 0;
        }
        else
        {
            throw new Exception("connection lost");
        }
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var query = $"DELETE FROM {_tableName} WHERE id = @Id";

        if (_db._connection.State == ConnectionState.Open)
        {
            var result = await _db._connection.ExecuteAsync(query, new { Id = id }, _db._transaction);
            return result > 0;
        }
        else
        {
            throw new Exception("connection lost");
        }
    }

    protected Dictionary<string, object> GetProperties(T entity, bool excludeKey = false)
    {
        var properties = typeof(T)
        .GetProperties()
        .Where(p => !excludeKey || p.Name != "Id")
        .Where(p => p.GetValue(entity) != null)
        .Where(p => !IsComplexType(p.PropertyType))
        .ToDictionary(p => p.Name, p => p.GetValue(entity));

        return properties;
    }
    private bool IsComplexType(Type type)
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

        if (type.IsClass &&
            type != typeof(string) &&
            type != typeof(DateTime) &&
            type != typeof(decimal))
        {
            return true;
        }

        return false;
    }

}


