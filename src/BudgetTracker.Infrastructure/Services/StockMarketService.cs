using BudgetTracker.Core.Infrastructure.Services;
using Microsoft.Extensions.Configuration;


namespace BudgetTracker.Infrastructure.Services
{
    public class StockMarketService(IHttpClientFactory httpClientFactory, IConfiguration configuration) : IStockMarketService
    {
        private readonly string urlBase = configuration["HGBrasil:Url"];
        private readonly string key = configuration["HGBrasil:Key"];

        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
        // TODO(Stock WIP): implement batch quote lookup; stubbed to satisfy IStockMarketService.
        public Task<string> GetStockByTickerAsync(List<string> ticker)
            => throw new NotImplementedException("Stock feature WIP.");

        public async Task<string> GetStockByTickerAsync(string ticker)
        {
            var client = _httpClientFactory.CreateClient();

            client.DefaultRequestHeaders.Add("Authorization", configuration["HGBrasil:AuthToken"]);

            string url = $"{urlBase}/v2/finance/quotes?tickers=B3:{ticker}&key={key}";

            var response = await client.GetAsync(url);

            return await response.Content.ReadAsStringAsync();
        }
    }
}
