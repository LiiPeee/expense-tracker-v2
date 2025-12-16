using Dapper;
using ExpenseTrackerV2.Core.Domain.Entities;
using ExpenseTrackerV2.Core.Domain.Repository;
using System.Data;

namespace ExpenseTrackerV2.Infrastructure.Persistence.Repository;

public class TransactionsRepository : RepositoryBase<Transactions>, ITransactionsRepository
{
    private readonly IDbConnection connection;
    private readonly IDbTransaction transaction;
    public TransactionsRepository(UnitOfWork unitOfWork)
    {
        connection = unitOfWork.Transaction.Connection;
        transaction = unitOfWork.Transaction;
    }
   

    public async Task<List<Transactions>> FilterTransactionsByCategoryAsync(long categoryId, long month)
    {
        var query =$"SELECT * FROM Transactions t WHERE t.CategoryId = @CategoryId AND MONTH(t.CreatedAt) = @Month ORDER BY 1 DESC";

        if (context. is not null) 
        {
            var transactions = (await _context.CurrentConnection.QueryAsync<Transactions>(query, new { CategoryId = categoryId, Month = month }, _context.CurrentTransaction));

            return transactions.ToList();
        }
        else
        {
            throw new Exception("connection lost");
        }
    }

    public async Task<List<Transactions>> FilterByMonthAsync(long month)
    {
        var query = @"SELECT * FROM Transactions t INNER JOIN Contact ctt ON ctt.Id = t.ContactId WHERE MONTH(t.CreatedAt) = @Month ORDER BY 1 DESC";

        if (_context.CurrentConnection is not null)
        {
            var transactions = _context.CurrentConnection.QueryAsync<Transactions, Contact>(query, (transactions, contact) =>
            {

            }).Result;
            return transactions.ToList();
        }
        else
        {
            throw new Exception("connection lost");
        }
    }
}
