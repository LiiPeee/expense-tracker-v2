using System.Data;
using BudgetTracker.Core.Domain.Dtos.Output;
using BudgetTracker.Core.Domain.Entities;
using BudgetTracker.Core.Infrastructure.Repository;
using Dapper;
using static System.Net.Mime.MediaTypeNames;

namespace BudgetTracker.Infrastructure.Persistence.Repository
{
    public class BudgetLimitRepository : RepositoryBase<BudgetLimit>, IBudgetLimitRepository
    {
        public BudgetLimitRepository(DbSession session) : base(session)
        {
        }

        public async Task<IPagedResult<BudgetLimit?>> GetByAccountIdAsync(long accountId, int pageNumber = 1)
        {
            const int pageSize = 10;
            const int maxPages = 10;

            pageNumber = Math.Clamp(pageNumber, 1, maxPages);
            var offset = (pageNumber - 1) * pageSize;

            var query = @"
                SELECT bt.*, cat.*
                FROM BudgetLimit bt
                INNER JOIN Category cat ON bt.CategoryId = cat.Id
                WHERE bt.AccountId = @AccountId
                ORDER BY bt.Id DESC
                LIMIT @PageSize OFFSET @OffSet;

                SELECT COUNT(1)
                FROM BudgetLimit bt
                WHERE bt.AccountId = @AccountId;";

            if (_db._connection.State != ConnectionState.Open)
            {
                throw new Exception("connection lost");
            }

            using var multi = await _db._connection.QueryMultipleAsync(
                query,
                new { AccountId = accountId, OffSet = offset, PageSize = pageSize },
                _db._transaction);

            var items = multi.Read<BudgetLimit, Category, BudgetLimit>(
                (c, ct) =>
                {
                    c.Category = ct;
                    return c;
                },
                splitOn: "Id,Id").ToList();

            var totalRecords = await multi.ReadSingleAsync<int>();

            return new IPagedResult<BudgetLimit?>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                Items = items
            };
        }

        public async Task<BudgetLimit?> GetByCategoryAndAccountIdAsync(string categoryName, long accountId)
        {
            var query = @"SELECT * FROM BudgetLimit bt 
            INNER JOIN Category ct ON ct.Id = bt.CategoryId
            INNER JOIN Account act ON act.Id = bt.AccountId
            WHERE ct.Name = @CategoryName AND act.Id = @AccountId";

            if(_db._connection.State != ConnectionState.Open)
            {
                throw new Exception("connection lost");
            }

            var result = (await _db._connection.QueryAsync<BudgetLimit, Category, Account, BudgetLimit>(query, (bt, c, a) =>
            {
                bt.Category = c;
                bt.Account = a;
                return bt;
            },
            new { AccountId = accountId, CategoryName = categoryName }, transaction: _db._transaction, splitOn: "Id,Id")).FirstOrDefault();

            return result;
        }
    }
}
