namespace BudgetTracker.Core.Infrastructure.OutPut
{
    public class StockMarketResponse
    {
        public string Ticker { get; set; }

        public decimal PriceMarket { get; set; }
    }

    public class BrApiResponse
    {
        public List<BrApiData> Results { get; set; }
    }

    public class BrApiData {
        public decimal RegularMarketPrice { get; set; }
        public decimal RegularMarketDayHigh { get; set; }
        public decimal RegularMarketDayLow { get; set; }
        public decimal RegularMarketChange { get; set; }
        public decimal RegularMarketChangePercent { get; set; }
        public decimal RegularMarketPreviousClose { get; set; }
        public string? LongName { get; set; }
        public string? Currency { get; set; }
        public long? MarketCap { get; set; }
        public long RegularMarketVolume { get; set; }
        public decimal FiftyTwoWeekLow { get; set; }
        public decimal FiftyTwoWeekHigh { get; set; }
    };

}
