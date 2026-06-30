using Dapper;
using BudgetTracker.Core.Domain.Dtos.Output;
using BudgetTracker.Core.Domain.Entities;
using BudgetTracker.Core.Domain.Enum;
using BudgetTracker.Core.Domain.Repository;
using System.Data;

namespace BudgetTracker.Infrastructure.Persistence.Repository;

public class TransactionsRepository : AccountScopedRepositoryBase<Transactions>, ITransactionsRepository
{
    public TransactionsRepository(DbSession connection) : base(connection)
    {
    }

    // FILTRO POR CATEGORIA, TIPO, MES E ANO
    public async Task<IPagedResult<Transactions>> FilterTransactionsByCategoryAsync(long accountId, string categoryName, string type, long month, long year, int pageNumber = 1)
    {
        const int pageSize = 10;

        pageNumber = Math.Max(1, pageNumber);
        var offset = (pageNumber - 1) * pageSize;

        var query = @"
        SELECT t.*, ct.*, cat.*
        FROM Transactions t
        INNER JOIN Contact ct ON t.ContactId = ct.Id
        INNER JOIN Category cat ON t.CategoryId = cat.Id
        INNER JOIN TypeTransaction tp ON t.TypeTransactionId = tp.Id
        WHERE t.AccountId = @AccountId AND tp.Name = @Type AND cat.Name = @Category 
            AND (EXTRACT(MONTH FROM t.CompetenceDate) = @Month AND EXTRACT(YEAR FROM t.CompetenceDate) = @Year)
        ORDER BY t.Id DESC
        LIMIT @PageSize OFFSET @OffSet;

        SELECT COUNT(1)
        FROM Transactions t
        INNER JOIN Contact ct ON t.ContactId = ct.Id
        INNER JOIN Category cat ON t.CategoryId = cat.Id
        INNER JOIN TypeTransaction tp ON t.TypeTransactionId = tp.Id
        WHERE t.AccountId = @AccountId AND tp.Name = @Type AND cat.Name = @Category 
            AND (EXTRACT(MONTH FROM t.CompetenceDate) = @Month AND EXTRACT(YEAR FROM t.CompetenceDate) = @Year);";

        if (_db._connection.State != ConnectionState.Open)
        {
            throw new Exception("connection lost");
        }

        using var multi = await _db._connection.QueryMultipleAsync(
            query,
            new { AccountId = accountId, Month = month, Year = year, OffSet = offset, PageSize = pageSize, Type = type, Category = categoryName },
            _db._transaction);

        var items = multi.Read<Transactions, Contact, Category, Transactions>(
            (t, c, cat) =>
            {
                t.Contact = c;
                t.Category = cat;
                return t;
            },
            splitOn: "Id,Id").ToList();

        var totalRecords = items.Count;

        return new IPagedResult<Transactions>
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalRecords = totalRecords,
            Items = items
        };
    }

    public async Task<IPagedResult<Transactions>> FilterTransactionsByTypeAsync(long accountId, string type, long month, long year, int pageNumber = 1)
    {
        const int pageSize = 10;

        pageNumber = Math.Max(1, pageNumber);
        var offset = (pageNumber - 1) * pageSize;

        var query = @"
        SELECT t.*, ct.*, cat.*
        FROM Transactions t
        INNER JOIN Contact ct ON t.ContactId = ct.Id
        INNER JOIN Category cat ON t.CategoryId = cat.Id
        INNER JOIN TypeTransaction tp ON t.TypeTransactionId = tp.Id
        WHERE t.AccountId = @AccountId AND tp.Name = @Type 
            AND (EXTRACT(MONTH FROM t.CompetenceDate) = @Month AND EXTRACT(YEAR FROM t.CompetenceDate) = @Year)
        ORDER BY t.Id DESC
        LIMIT @PageSize OFFSET @OffSet;

        SELECT COUNT(1)
        FROM Transactions t
        INNER JOIN Contact ct ON t.ContactId = ct.Id
        INNER JOIN Category cat ON t.CategoryId = cat.Id
        INNER JOIN TypeTransaction tp ON t.TypeTransactionId = tp.Id
        WHERE t.AccountId = @AccountId AND tp.Name = @Type 
            AND (EXTRACT(MONTH FROM t.CompetenceDate) = @Month AND EXTRACT(YEAR FROM t.CompetenceDate) = @Year);";

        if (_db._connection.State != ConnectionState.Open)
        {
            throw new Exception("connection lost");
        }

        using var multi = await _db._connection.QueryMultipleAsync(
            query,
            new { AccountId = accountId, Month = month, Year = year, OffSet = offset, PageSize = pageSize, Type = type },
            _db._transaction);

        var items = multi.Read<Transactions, Contact, Category, Transactions>(
            (t, c, cat) =>
            {
                t.Contact = c;
                t.Category = cat;
                return t;
            },
            splitOn: "Id,Id").ToList();

        var totalRecords = items.Count;

        return new IPagedResult<Transactions>
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalRecords = totalRecords,
            Items = items
        };
    }

    public async Task<IPagedResult<Transactions>> FilterByMonthAndYearAsync(long accountId, long month, long year, int pageNumber = 1)
    {
        const int pageSize = 10;

        pageNumber = Math.Max(1, pageNumber);
        var offset = (pageNumber - 1) * pageSize;

        var query = @"
        SELECT t.*, ct.*, cat.*
        FROM Transactions t
        INNER JOIN Contact ct ON t.ContactId = ct.Id
        INNER JOIN Category cat ON t.CategoryId = cat.Id
        WHERE t.AccountId = @AccountId 
            AND (EXTRACT(MONTH FROM t.CompetenceDate) = @Month AND EXTRACT(YEAR FROM t.CompetenceDate) = @Year)
        ORDER BY t.Id DESC
        LIMIT @PageSize OFFSET @OffSet;

        SELECT COUNT(1)
        FROM Transactions t
        INNER JOIN Contact ct ON t.ContactId = ct.Id
        INNER JOIN Category cat ON t.CategoryId = cat.Id
        WHERE t.AccountId = @AccountId 
            AND (EXTRACT(MONTH FROM t.CompetenceDate) = @Month AND EXTRACT(YEAR FROM t.CompetenceDate) = @Year);";

        if (_db._connection.State != ConnectionState.Open)
        {
            throw new Exception("connection lost");
        }

        using var multi = await _db._connection.QueryMultipleAsync(
            query,
            new { AccountId = accountId, Month = month, Year = year, OffSet = offset, PageSize = pageSize },
            _db._transaction);

        var items = multi.Read<Transactions, Contact, Category, Transactions>(
            (t, c, cat) =>
            {
                t.Contact = c;
                t.Category = cat;
                return t;
            },
            splitOn: "Id,Id").ToList();

        var totalRecords = items.Count;

        return new IPagedResult<Transactions>
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalRecords = totalRecords,
            Items = items
        };
    }

    public async Task<IPagedResult<Transactions>> FilterByContactAsync(long accountId, long year, long month, string type, long contactId, int pageNumber = 1)
    {

        const int pageSize = 10;

        pageNumber = Math.Max(1, pageNumber);
        var offset = (pageNumber - 1) * pageSize;

        var query = @"
        SELECT t.*, ct.*, cat.*
        FROM Transactions t
        INNER JOIN Contact ct ON t.ContactId = ct.Id
        INNER JOIN Category cat ON t.CategoryId = cat.Id
        INNER JOIN TypeTransaction tp ON t.TypeTransactionId = tp.Id
        WHERE t.AccountId = @AccountId AND tp.Name = @Type 
            AND ct.Id = @ContactId
            AND (EXTRACT(MONTH FROM t.CompetenceDate) = @Month AND EXTRACT(YEAR FROM t.CompetenceDate) = @Year)
        ORDER BY t.Id DESC
        LIMIT @PageSize OFFSET @OffSet;

        SELECT COUNT(1)
        FROM Transactions t
        INNER JOIN Contact ct ON t.ContactId = ct.Id
        INNER JOIN Category cat ON t.CategoryId = cat.Id
        INNER JOIN TypeTransaction tp ON t.TypeTransactionId = tp.Id
        WHERE t.AccountId = @AccountId AND tp.Name = @Type 
            AND (EXTRACT(MONTH FROM t.CompetenceDate) = @Month AND EXTRACT(YEAR FROM t.CompetenceDate) = @Year);";

        if (_db._connection.State != ConnectionState.Open)
        {
            throw new Exception("connection lost");
        }

        using var multi = await _db._connection.QueryMultipleAsync(
            query,
            new { AccountId = accountId, Month = month, Year = year, OffSet = offset, PageSize = pageSize, ContactId = contactId, Type = type },
            _db._transaction);

        var items = multi.Read<Transactions, Contact, Category, Transactions>(
            (t, c, cat) =>
            {
                t.Contact = c;
                t.Category = cat;
                return t;
            },
            splitOn: "Id,Id").ToList();

        var totalRecords = items.Count;

        return new IPagedResult<Transactions>
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalRecords = totalRecords,
            Items = items
        };
    }

    public async Task<List<Transactions>> FilterExpenseMonthAndYearAsync(long accountId, long year, long month)
    {
        var query = @"SELECT * FROM Transactions t
        WHERE t.AccountId = @AccountId 
            AND (EXTRACT(MONTH FROM t.CompetenceDate) = @Month AND EXTRACT(YEAR FROM t.CompetenceDate) = @Year)
            AND t.TypeTransactionId = @TypeId";

        if (_db._connection.State == ConnectionState.Open)
        {
            var result = await _db._connection.QueryAsync<Transactions>(query, new { AccountId = accountId, Month = month, Year = year, TypeId = (long)TypeTransactions.EXPENSE }, _db._transaction);
            return result.ToList();
        }
        else
        {
            throw new Exception("connection lost");
        }
    }

    public async Task<List<Transactions>> FilterIncomeMonthAndYearAsync(long accountId, long year, long month)
    {
        var query = @"SELECT * FROM Transactions t
        WHERE t.AccountId = @AccountId 
            AND (EXTRACT(MONTH FROM t.CompetenceDate) = @Month AND EXTRACT(YEAR FROM t.CompetenceDate) = @Year)
            AND t.TypeTransactionId = @TypeId";

        if (_db._connection.State == ConnectionState.Open)
        {
            var result = await _db._connection.QueryAsync<Transactions>(query, new { AccountId = accountId, Month = month, Year = year, TypeId = (long)TypeTransactions.INCOME }, _db._transaction);
            return result.ToList();
        }
        else
        {
            throw new Exception("connection lost");
        }
    }

    public async Task<List<Transactions>> FilterExpenseMonthWithContactAsync(long accountId, long year, long month)
    {
        var query = @"SELECT t.*, ct.*
        FROM Transactions t 
        LEFT JOIN Contact ct ON t.ContactId = ct.Id
        WHERE t.AccountId = @AccountId 
            AND (EXTRACT(MONTH FROM t.CompetenceDate) = @Month AND EXTRACT(YEAR FROM t.CompetenceDate) = @Year)";

        if (_db._connection.State == ConnectionState.Open)
        {
            var result = await _db._connection.QueryAsync<Transactions, Contact, Transactions>(query, (t, c) =>
            {
                t.Contact = c;
                return t;
            },
            new { AccountId = accountId, Month = month, Year = year }, transaction: _db._transaction, splitOn: "Id");

            return result.ToList();
        }
        else
        {
            throw new Exception("connection lost");
        }
    }

    public async Task<bool> MarkAsPaidAsync(long id, long accountId)
    {
        const string query = @"UPDATE Transactions SET Paid = true WHERE Id = @Id AND AccountId = @AccountId AND Paid = false";

        if (_db._connection.State != ConnectionState.Open)
            throw new Exception("connection lost");

        var rows = await _db._connection.ExecuteAsync(query, new { Id = id, AccountId = accountId }, _db._transaction);
        return rows > 0;
    }

    public async Task<decimal> GetExpenseTotalByCategoryAsync(long accountId, long categoryId, int month, int year)
    {
        const string query = @"
        SELECT COALESCE(SUM(t.Amount), 0)
        FROM Transactions t
        WHERE t.AccountId = @AccountId
            AND t.CategoryId = @CategoryId
            AND t.TypeTransactionId = @ExpenseType
            AND (EXTRACT(MONTH FROM t.CompetenceDate) = @Month AND EXTRACT(YEAR FROM t.CompetenceDate) = @Year)";

        if (_db._connection.State != ConnectionState.Open)
            throw new Exception("connection lost");

        return await _db._connection.ExecuteScalarAsync<decimal>(query, new
        {
            AccountId = accountId,
            CategoryId = categoryId,
            Month = month,
            Year = year,
            ExpenseType = (long)TypeTransactions.EXPENSE
        }, _db._transaction);
    }

}


