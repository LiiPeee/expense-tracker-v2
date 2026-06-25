using BudgetTracker.Application.Service;
using BudgetTracker.Core.Domain.Entities;
using BudgetTracker.Core.Domain.UnitOfWork;
using BudgetTracker.Core.Infrastructure.OutPut;
using BudgetTracker.Core.Infrastructure.Repository;
using BudgetTracker.Core.Infrastructure.Services;
using Moq;

namespace Test.Application;

/// <summary>
/// A missing market quote (rate limit, unknown ticker, API outage) must degrade to
/// price 0 — never throw — so the whole portfolio still loads.
/// </summary>
[TestFixture]
public class StockAppServiceTests
{
    private const long AccountId = 1;

    private static Stock BuildStock(string ticker, decimal priceBuyed) =>
        new() { Id = 1, AccountId = AccountId, Ticker = ticker, Title = ticker, PriceBuyed = priceBuyed, Quantity = 1 };

    private static StockAppService BuildService(List<Stock> stocks, List<StockMarketResponse> live)
    {
        var stockRepository = new Mock<IStockRepository>();
        stockRepository.Setup(r => r.GetAllAsync(AccountId)).ReturnsAsync(stocks);
        stockRepository.Setup(r => r.UpdateAsync(It.IsAny<Stock>())).ReturnsAsync(true);

        var marketService = new Mock<IStockMarketService>();
        marketService.Setup(m => m.GetStockByTickerAsync(It.IsAny<List<string>>())).ReturnsAsync(live);

        return new StockAppService(stockRepository.Object, marketService.Object, new Mock<IUnitOfWork>().Object);
    }

    [Test]
    public async Task Dado_MercadoSemCotacao_Quando_ListarAtivos_Entao_RetornaComPrecoZeroSemErro()
    {
        var service = BuildService(new List<Stock> { BuildStock("PETR4", 10m) }, new List<StockMarketResponse>());

        var result = await service.GetAllStockAsync(AccountId, 1);

        Assert.Multiple(() =>
        {
            Assert.That(result.Items.Count(), Is.EqualTo(1));
            Assert.That(result.Items.First().Ticker, Is.EqualTo("PETR4"));
            Assert.That(result.Items.First().PriceMarket, Is.EqualTo(0));
        });
    }

    [Test]
    public async Task Dado_CotacaoParcial_Quando_ListarAtivos_Entao_NaoLancaParaTickerSemCotacao()
    {
        var service = BuildService(
            new List<Stock> { BuildStock("PETR4", 10m), BuildStock("VALE3", 20m) },
            new List<StockMarketResponse> { new() { Ticker = "PETR4", PriceMarket = 15m } });

        var result = await service.GetAllStockAsync(AccountId, 1);

        Assert.Multiple(() =>
        {
            Assert.That(result.Items.Count(), Is.EqualTo(2));
            Assert.That(result.Items.First(s => s.Ticker == "PETR4").PriceMarket, Is.EqualTo(15m));
            Assert.That(result.Items.First(s => s.Ticker == "VALE3").PriceMarket, Is.EqualTo(0));
        });
    }
}
