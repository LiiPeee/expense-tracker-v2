namespace BudgetTracker.Core.Domain.Models.Output;

public class GetAllStockResponse
{
    public string Ticker { get; set; }

    public decimal PriceMarket { get; set; }

    public decimal PriceBuyed { get; set; }

    public long Quantity { get; set; }

    public string Percentage { get; set; }
}