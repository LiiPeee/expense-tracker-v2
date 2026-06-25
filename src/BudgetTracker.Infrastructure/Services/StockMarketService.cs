using System.Net.Http.Json;
using System.Text.Json;
using BudgetTracker.Core.Infrastructure.OutPut;
using BudgetTracker.Core.Infrastructure.Services;
using Microsoft.Extensions.Configuration;


namespace BudgetTracker.Infrastructure.Services
{
    public class StockMarketService(IHttpClientFactory httpClientFactory, IConfiguration configuration) : IStockMarketService
    {
        private readonly string _urlBase = configuration["BrApi:Url"]!;
        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;

        private static readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public async Task<List<StockMarketResponse>> GetStockByTickerAsync(List<string> ticker)
        {
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {configuration["BrApi:Key"]}");

            var stocks = new List<StockMarketResponse>();

            foreach (var tick in ticker)
            {
                try
                {
                    var response = await client.GetAsync($"{_urlBase}stocks/quote?symbols={tick}");

                    if (!response.IsSuccessStatusCode)
                        continue;

                    var result = await response.Content.ReadFromJsonAsync<BrApiResponse>(_jsonOptions);
                    var price = result?.Results?.FirstOrDefault()?.Data;

                    if (price is null)
                        continue;

                    stocks.Add(new() { Ticker = tick, PriceMarket = price.RegularMarketPrice });
                }
                catch
                {
                    // Skip tickers the market API can't resolve so a single failure
                    // doesn't break the whole portfolio fetch.
                }
            }

            return stocks;
        }

        private record BrApiResponse(List<BrApiResult>? Results);

        private record BrApiResult(string Symbol, BrApiData? Data);

        private record BrApiData(
            decimal RegularMarketPrice,
            decimal RegularMarketDayHigh,
            decimal RegularMarketDayLow,
            decimal RegularMarketChange,
            decimal RegularMarketChangePercent,
            decimal RegularMarketPreviousClose,
            string? LongName,
            string? Currency,
            long MarketCap,
            long RegularMarketVolume,
            decimal FiftyTwoWeekLow,
            decimal FiftyTwoWeekHigh
        );
    }
}
