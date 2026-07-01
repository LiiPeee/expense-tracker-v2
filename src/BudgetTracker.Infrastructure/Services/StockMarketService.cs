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

        private async Task<StockMarketResponse?> FetchPriceAsync(HttpClient client, string tick)
        {
            try
            {
                var response = await client.GetAsync($"{_urlBase}quote/{tick}");
                if (!response.IsSuccessStatusCode) return null;

                var result = await response.Content.ReadFromJsonAsync<BrApiResponse>(_jsonOptions);
                var price = result?.Results?.FirstOrDefault()?.RegularMarketPrice ?? 0;

                return price > 0 ? new StockMarketResponse { Ticker = tick, PriceMarket = price } : null;
            }
            catch
            {
                return null;
            }
        }

        public async Task<List<StockMarketResponse>> GetStockByTickerAsync(List<string> ticker)
        {
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {configuration["BrApi:Key"]}");

            var results = await Task.WhenAll(ticker.Select(tick => FetchPriceAsync(client, tick)));

            return results.Where(r => r is not null).Select(r => r!).ToList();
        }

        public async Task<List<StockMarketResponse>> GetFundsByTickerAsync(List<string> ticker)
        {
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {configuration["BrApi:Key"]}");

            var results = await Task.WhenAll(ticker.Select(tick => FetchPriceAsync(client, tick)));

            return results.Where(r => r is not null).Select(r => r!).ToList();
        }
    }
}
