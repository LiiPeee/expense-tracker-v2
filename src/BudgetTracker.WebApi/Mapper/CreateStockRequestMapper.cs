using BudgetTracker.Core.Domain.Entities;
using BudgetTracker.Core.Domain.Models.Request;
using BudgetTracker.Core.Domain.Models.Request.BudgetLimit;
using BudgetTracker.WebApi.Models.BudgetLimit;
using BudgetTracker.WebApi.Models.Stock;

namespace BudgetTracker.WebApi.Mapper
{
    public static class CreateStockRequestMapper
    {
        public static CreateStockRequest ToCreateStock(this CreateStock request)

        {
            return new CreateStockRequest
            {
                Ticker = request.Ticker,
                Price = request.Price,
                Title = request.Title,
                Description = request.Description,
                Quantity = request.Quantity,
            };
        }
    }
}
