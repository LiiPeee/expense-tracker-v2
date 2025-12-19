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
   

    public async Task<List<Transactions>> FilterTransactionsByCategoryAsync(long categoryId, long month)
    {
        var query =$"SELECT * FROM Transactions t WHERE t.CategoryId = @CategoryId AND MONTH(t.CreatedAt) = @Month ORDER BY 1 DESC";

        if (_db._connection.State == ConnectionState.Open) 
        {
            var transactions = (await _db._connection.QueryAsync<Transactions>(query, new { CategoryId = categoryId, Month = month }, transaction : _db._transaction));

            return transactions.ToList();
        }
        else
        {
            throw new Exception("connection lost");
        }
    }

    public async Task<List<Transactions>> FilterByMonthAndYearAsync(long month, long year)
    {
        var query = @"SELECT * FROM Transactions t WHERE MONTH(t.CreatedAt) AND YEAR(t.CreatedAt) AND MONTH(t.DateOfInstallment) AND YEAR(t.DateOfInstallment) ORDER BY 1 DESC";

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

    public async Task<List<Transactions>> FilterByMonthAndContact(long month, long contactId)
    {
        var query = @"SELECT * FROM Transactions t INNER JOIN Contact ctt ON t.ContactId = ctt.Id WHERE MONTH(t.DateOfInstallment) = @Month AND ctt.Id = @ContactId";

        if (_db._connection.State == ConnectionState.Open)
        {

            var result = await _db._connection.QueryAsync<Transactions, Contact, Transactions>(query, (t, c) =>
            {
                t.Contact = c;
                return t;
            }, new { Month = month, ContactId = contactId }, _db._transaction, splitOn: "Id");

            return result.ToList();
        }
        else
        {
            throw new Exception("somenthing wrong ocurr in DB"); 
        }
    }
}
