using BudgetTracker.Core.Infrastructure.OutPut;
using BudgetTracker.Core.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using System.Globalization;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace BudgetTracker.Infrastructure.Services
{
    public class BacenService(IHttpClientFactory httpClientFactory, IConfiguration configuration) : IBacenService
    {
        private readonly string _urlBase = configuration["Bacen:Url"]!;
        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;

        private static readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };


        public async Task<IEnumerable<BacenOutPut>> GetHistoryCdiAsync(string from, string to)
        {
            var client = _httpClientFactory.CreateClient();


            var response = await client.GetAsync($"{_urlBase}/dados/serie/bcdata.sgs.12/dados?formato=json&dataInicial={from}&dataFinal={to}");

            var data = await response.Content.ReadFromJsonAsync<List<BacenCdiEntry>>(_jsonOptions);

            var output = new List<BacenOutPut>();


            var result = data!.Select(e => 
               new BacenOutPut()
               {

                    Date =   DateOnly.ParseExact(e.Data, "dd/MM/yyyy").ToString("yyyy-MM-dd"),
                    Value =decimal.Parse(e.Valor, CultureInfo.InvariantCulture)

               }
            );

            return result;
        }
        

        public record BacenCdiEntry(
        [property: JsonPropertyName("data")] string Data,   
        [property: JsonPropertyName("valor")] string Valor  
        );
    }
}
