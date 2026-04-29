using Dapper;
using ExpenseTrackerV2.Core.Domain.Dtos.Output;
using ExpenseTrackerV2.Core.Domain.Entities;
using ExpenseTrackerV2.Core.Domain.Repository;
using System.Data;

namespace ExpenseTrackerV2.Infrastructure.Persistence.Repository;

public class TransactionsRepository : RepositoryBase<Transactions>, ITransactionsRepository
{
    public TransactionsRepository(DbSession connection) : base(connection)
    {
    }

    // FILTRO POR CATEGORIA, TIPO, MES E ANO
    public async Task<IPagedResult<Transactions>> FilterTransactionsByCategoryAsync(long accountId, string categoryName, string type, long month, long year, int pageNumber = 1)
    {
        const int pageSize = 10;
        const int maxPages = 10;

        pageNumber = Math.Clamp(pageNumber, 1, maxPages);
        var offset = (pageNumber - 1) * pageSize;

        var query = @"
        SELECT t.*, ct.*, cat.*
        FROM Transactions t
        INNER JOIN Contact ct ON t.ContactId = ct.Id
        INNER JOIN Category cat ON t.CategoryId = cat.Id
        INNER JOIN TypeTransaction tp ON t.TypeTransactionId = tp.Id
        WHERE t.AccountId = @AccountId AND tp.Name = @Type AND cat.Name = @Category 
            AND ((t.DateOfInstallment IS NULL AND MONTH(t.CreatedAt) = @Month AND YEAR(t.CreatedAt) = @Year)
            OR (t.DateOfInstallment IS NOT NULL AND MONTH(t.DateOfInstallment) = @Month AND YEAR(t.DateOfInstallment) = @Year))
        ORDER BY t.Id DESC
        OFFSET @OffSet ROWS FETCH NEXT @PageSize ROWS ONLY;

        SELECT COUNT(1)
        FROM Transactions t
        INNER JOIN Contact ct ON t.ContactId = ct.Id
        INNER JOIN Category cat ON t.CategoryId = cat.Id
        INNER JOIN TypeTransaction tp ON t.TypeTransactionId = tp.Id
        WHERE t.AccountId = @AccountId AND tp.Name = @Type AND cat.Name = @Category 
            AND ((t.DateOfInstallment IS NULL AND MONTH(t.CreatedAt) = @Month AND YEAR(t.CreatedAt) = @Year)
            OR (t.DateOfInstallment IS NOT NULL AND MONTH(t.DateOfInstallment) = @Month AND YEAR(t.DateOfInstallment) = @Year));";

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

        var totalRecords = await multi.ReadSingleAsync<int>();

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
        const int maxPages = 10;

        pageNumber = Math.Clamp(pageNumber, 1, maxPages);
        var offset = (pageNumber - 1) * pageSize;

        var query = @"
        SELECT t.*, ct.*, cat.*
        FROM Transactions t
        INNER JOIN Contact ct ON t.ContactId = ct.Id
        INNER JOIN Category cat ON t.CategoryId = cat.Id
        INNER JOIN TypeTransaction tp ON t.TypeTransactionId = tp.Id
        WHERE t.AccountId = @AccountId AND tp.Name = @Type 
            AND ((t.DateOfInstallment IS NULL AND MONTH(t.CreatedAt) = @Month AND YEAR(t.CreatedAt) = @Year)
            OR (t.DateOfInstallment IS NOT NULL AND MONTH(t.DateOfInstallment) = @Month AND YEAR(t.DateOfInstallment) = @Year))
        ORDER BY t.Id DESC
        OFFSET @OffSet ROWS FETCH NEXT @PageSize ROWS ONLY;

        SELECT COUNT(1)
        FROM Transactions t
        INNER JOIN Contact ct ON t.ContactId = ct.Id
        INNER JOIN Category cat ON t.CategoryId = cat.Id
        INNER JOIN TypeTransaction tp ON t.TypeTransactionId = tp.Id
        WHERE t.AccountId = @AccountId AND tp.Name = @Type 
            AND ((t.DateOfInstallment IS NULL AND MONTH(t.CreatedAt) = @Month AND YEAR(t.CreatedAt) = @Year)
            OR (t.DateOfInstallment IS NOT NULL AND MONTH(t.DateOfInstallment) = @Month AND YEAR(t.DateOfInstallment) = @Year));";

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

        var totalRecords = await multi.ReadSingleAsync<int>();

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
        const int maxPages = 10;

        pageNumber = Math.Clamp(pageNumber, 1, maxPages);
        var offset = (pageNumber - 1) * pageSize;

        var query = @"
        SELECT t.*, ct.*, cat.*
        FROM Transactions t
        INNER JOIN Contact ct ON t.ContactId = ct.Id
        INNER JOIN Category cat ON t.CategoryId = cat.Id
        WHERE t.AccountId = @AccountId 
            AND ((t.DateOfInstallment IS NULL AND MONTH(t.CreatedAt) = @Month AND YEAR(t.CreatedAt) = @Year)
            OR (t.DateOfInstallment IS NOT NULL AND MONTH(t.DateOfInstallment) = @Month AND YEAR(t.DateOfInstallment) = @Year))
        ORDER BY t.Id DESC
        OFFSET @OffSet ROWS FETCH NEXT @PageSize ROWS ONLY;

        SELECT COUNT(1)
        FROM Transactions t
        INNER JOIN Contact ct ON t.ContactId = ct.Id
        INNER JOIN Category cat ON t.CategoryId = cat.Id
        WHERE t.AccountId = @AccountId 
            AND ((t.DateOfInstallment IS NULL AND MONTH(t.CreatedAt) = @Month AND YEAR(t.CreatedAt) = @Year)
            OR (t.DateOfInstallment IS NOT NULL AND MONTH(t.DateOfInstallment) = @Month AND YEAR(t.DateOfInstallment) = @Year));";

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

        var totalRecords = await multi.ReadSingleAsync<int>();

        return new IPagedResult<Transactions>
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalRecords = totalRecords,
            Items = items
        };
    }

    public async Task<IPagedResult<Transactions>> FilterByMonthAndContactAsync(long accountId, long year, long month, string type, string contactName, int pageNumber = 1)
    {

        const int pageSize = 10;
        const int maxPages = 10;

        pageNumber = Math.Clamp(pageNumber, 1, maxPages);
        var offset = (pageNumber - 1) * pageSize;

        var query = @"
        SELECT t.*, ct.*, cat.*
        FROM Transactions t
        INNER JOIN Contact ct ON t.ContactId = ct.Id
        INNER JOIN Category cat ON t.CategoryId = cat.Id
        INNER JOIN TypeTransaction tp ON t.TypeTransactionId = tp.Id
        WHERE t.AccountId = @AccountId AND tp.Name = @Type 
            AND ct.Name = @ContactName
            AND ((t.DateOfInstallment IS NULL AND MONTH(t.CreatedAt) = @Month AND YEAR(t.CreatedAt) = @Year)
            OR (t.DateOfInstallment IS NOT NULL AND MONTH(t.DateOfInstallment) = @Month AND YEAR(t.DateOfInstallment) = @Year))
        ORDER BY t.Id DESC
        OFFSET @OffSet ROWS FETCH NEXT @PageSize ROWS ONLY;

        SELECT COUNT(1)
        FROM Transactions t
        INNER JOIN Contact ct ON t.ContactId = ct.Id
        INNER JOIN Category cat ON t.CategoryId = cat.Id
        INNER JOIN TypeTransaction tp ON t.TypeTransactionId = tp.Id
        WHERE t.AccountId = @AccountId AND tp.Name = @Type 
            AND ((t.DateOfInstallment IS NULL AND MONTH(t.CreatedAt) = @Month AND YEAR(t.CreatedAt) = @Year)
            OR (t.DateOfInstallment IS NOT NULL AND MONTH(t.DateOfInstallment) = @Month AND YEAR(t.DateOfInstallment) = @Year));";

        if (_db._connection.State != ConnectionState.Open)
        {
            throw new Exception("connection lost");
        }

        using var multi = await _db._connection.QueryMultipleAsync(
            query,
            new { AccountId = accountId, Month = month, Year = year, OffSet = offset, PageSize = pageSize, ContactName = contactName,Type = type },
            _db._transaction);

        var items = multi.Read<Transactions, Contact, Category, Transactions>(
            (t, c, cat) =>
            {
                t.Contact = c;
                t.Category = cat;
                return t;
            },
            splitOn: "Id,Id").ToList();

        var totalRecords = await multi.ReadSingleAsync<int>();

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
            AND ((t.DateOfInstallment IS NULL AND MONTH(t.CreatedAt) = @Month AND YEAR(t.CreatedAt) = @Year) 
            OR (t.DateOfInstallment IS NOT NULL AND MONTH(t.DateOfInstallment) = @Month AND YEAR(t.DateOfInstallment) = @Year))
            AND t.TypeTransactionId = 1";

        if (_db._connection.State == ConnectionState.Open)
        {
            var result = await _db._connection.QueryAsync<Transactions>(query, new { AccountId = accountId, Month = month, Year = year }, _db._transaction);
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
            AND ((t.DateOfInstallment IS NULL AND MONTH(t.CreatedAt) = @Month AND YEAR(t.CreatedAt) = @Year) 
            OR (t.DateOfInstallment IS NOT NULL AND MONTH(t.DateOfInstallment) = @Month AND YEAR(t.DateOfInstallment) = @Year))
            AND t.TypeTransactionId = 2";

        if (_db._connection.State == ConnectionState.Open)
        {
            var result = await _db._connection.QueryAsync<Transactions>(query, new { AccountId = accountId, Month = month, Year = year }, _db._transaction);
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
            AND ((MONTH(t.CreatedAt) = @Month AND YEAR(t.CreatedAt) = @Year) 
            OR (MONTH(t.DateOfInstallment) = @Month AND YEAR(t.DateOfInstallment) = @Year))";

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

    public async Task DeleteTransactionAsync(long accountId, long id)
    {
        var query = @"DELETE FROM Transactions WHERE Id = @Id AND AccountId = @AccountId";

        if (_db._connection.State == ConnectionState.Open)
        {
            await _db._connection.ExecuteAsync(query, new { Id = id, AccountId = accountId }, _db._transaction);
        }
        else
        {
            throw new Exception("connection lost");
        }
    }
}
