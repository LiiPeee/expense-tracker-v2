using Dapper;
using ExpenseTrackerV2.Core.Domain.Entities;
using ExpenseTrackerV2.Core.Domain.Repository;
using System.Data;

namespace ExpenseTrackerV2.Infrastructure.Persistence.Repository;

public class TransactionsRepository : RepositoryBase<Transactions>, ITransactionsRepository
{
   
    public TransactionsRepository(DbSession connection): base(connection)
    {
        
    }
   

    public async Task<List<Transactions>> FilterTransactionsByCategoryAsync(long categoryId, long month, long year)
    {
        var query =@"SELECT * FROM Transactions t 
        WHERE t.CategoryId = @CategoryId AND ((t.DateOfInstallment IS NOT NULL AND MONTH(t.DateOfInstallment) = @Month AND YEAR(t.DateOfInstallment) = @Year) 
        OR (t.DateOfInstallment IS NULL AND MONTH(t.CreatedAt) = @Month AND YEAR(t.CreatedAt) = @Year)) ORDER BY 1 DESC";

        if (_db._connection.State == ConnectionState.Open) 
        {
            var transactions = (await _db._connection.QueryAsync<Transactions>(query, new { CategoryId = categoryId, Month = month, Year = year}, transaction : _db._transaction));

            return transactions.ToList();
        }
        else
        {
            throw new Exception("connection lost");
        }
    }

    public async Task<List<Transactions>> FilterByMonthAndYearAsync(long month, long year)
    {
        var query = @"SELECT * FROM Transactions t 
        WHERE ((MONTH(t.CreatedAt) = @Month AND YEAR(t.CreatedAt) = @Year)  
        OR (MONTH(t.DateOfInstallment) = @Month AND YEAR(t.DateOfInstallment) = @Year)) ORDER BY 1 DESC";

        if (_db._connection.State == ConnectionState.Open)
        {
            var transactions = await _db._connection.QueryAsync<Transactions>(query, 
            new {Month = month, Year = year}, _db._transaction);

            return transactions.ToList();
        }
        else
        {
            throw new Exception("connection lost");
        }
    }

    public async Task<List<Transactions>> FilterByMonthAndContact(long year,long month, long contactId)
    {
        var query = @"SELECT * FROM Transactions t 
        INNER JOIN Contact ctt ON t.ContactId = ctt.Id 
        WHERE ((MONTH(t.DateOfInstallment) = @Month AND YEAR(t.DateOfInstallment) = @Year) OR 
        (MONTH(t.CreatedAt) = @Month AND YEAR(t.CreatedAt) = @Year)) AND ctt.Id = @ContactId";

        if (_db._connection.State == ConnectionState.Open)
        {

            var result = await _db._connection.QueryAsync<Transactions, Contact, Transactions>(query, (t, c) =>
            {
                t.Contact = c;
                return t;
            }, new { Month = month, ContactId = contactId, Year = year}, _db._transaction, splitOn: "Id");

            return result.ToList();
        }
        else
        {
            throw new Exception("somenthing wrong ocurr in DB"); 
        }
    }

    public async Task<List<Transactions>> FilterExpenseMonthAndYear(long year, long month)
    {
        var query = @"SELECT * FROM Transactions t
        LEFT JOIN TypeTransaction tp ON t.TypeTransactionId = tp.Id
        WHERE ((MONTH(t.CreatedAt) = @Month AND YEAR(t.CreatedAt) = @Year) OR 
        (MONTH(t.DateOfInstallment) = @Month AND YEAR(t.DateOfInstallment) = @Year))
        AND t.TypeTransactionId = 1";

        if (_db._connection.State == ConnectionState.Open)
        {

            var result = await _db._connection.QueryAsync<Transactions>(query, new { Month = month, Year = year }, _db._transaction);

            return result.ToList();
        }
        else
        {
            throw new Exception("somenthing wrong ocurr in DB");
        }
    }

    public async Task<List<Transactions>> FilterIncomeMonthAndYear(long year, long month)
    {
        var query = @"SELECT * FROM Transactions t
        LEFT JOIN TypeTransaction tp ON t.TypeTransactionId = tp.Id
        WHERE ((MONTH(t.CreatedAt) = @Month AND YEAR(t.CreatedAt) = @Year)
        OR (MONTH(t.DateOfInstallment) = @Month AND YEAR(t.DateOfInstallment) = @Year))
        AND t.TypeTransactionId = 2";

        if (_db._connection.State == ConnectionState.Open)
        {
            var result = await _db._connection.QueryAsync<Transactions>(query, new { Month = month, Year = year }, _db._transaction);

            return result.ToList();
        }
        else
        {
            throw new Exception("somenthing wrong ocurr in DB");
        }
    }
}
