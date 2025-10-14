using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Reflection;
using Dapper;
using ExpenseTrackerV2.Core.Domain.Repository;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTrackerV2.Infrastructure.Persistence.Repository;

public class RepositoryBase<T> : IRepositoryBase<T> where T : class
{
    protected readonly DapperContext _context;
    protected readonly string _tableName;
    public RepositoryBase(DapperContext context)
    {
        _context = context;
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

        var query = $"INSERT INTO {_tableName} ({columns}) VALUES ({values}); SELECT CAST(SCOPE_IDENTITY() as int)";

        using (var connection = _context.CreateConnection())
        {
            var id = await connection.ExecuteScalarAsync<int>(query, entity);

            var selectQuery = $"SELECT * FROM {_tableName} WHERE Id = @Id";
            return await connection.QueryFirstOrDefaultAsync<T>(selectQuery, new { Id = id });

        }
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        var query = $"SELECT * FROM {_tableName}";

        using (var connection = _context.CreateConnection())
        {
            connection.Open();

            var result = await connection.QueryAsync<T>(query);
            return result.ToList();
        }

    }

    public async Task<T?> GetByIdAsync(long id)
    {
        var query = $"SELECT * FROM {_tableName} WHERE id = @Id";

        using (var connection = _context.CreateConnection())
        {
            var result = await connection.QuerySingleOrDefaultAsync<T>(query, new { Id = id });

            return result;
        }
    }
    public async Task<bool> UpdateAsync(T entity)
    {
        var properties = GetProperties(entity, excludeKey: true);
        var setClause = string.Join(", ", properties.Keys.Select(k => $"{k} = @{k}"));

        var query = $"UPDATE {_tableName} SET {string.Join(", ", setClause)} WHERE id = @Id";

        using (var connection = _context.CreateConnection())
        {

            var result = await connection.ExecuteAsync(query, entity);

            return result > 0;
        }
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var query = $"DELETE FROM {_tableName} WHERE id = @Id";

        using (var connection = _context.CreateConnection())
        {
            connection.Open();

            var result = await connection.ExecuteAsync(query, new { Id = id });

            return result > 0;
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
