
namespace BudgetTracker.WebApi.Models.Stock
{

    public class CreateStock
    {
        public required string Ticker { get; set; }

        public required string Title { get; set; }

        public required decimal Price { get; set; }

        public long Quantity { get; set; }

        public string Description { get; set; }

    }
}