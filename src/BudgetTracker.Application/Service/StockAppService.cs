using BudgetTracker.Core.Domain.Dtos.Output;
using BudgetTracker.Core.Domain.Entities;
using BudgetTracker.Core.Domain.Models.Output;
using BudgetTracker.Core.Domain.Models.Request;
using BudgetTracker.Core.Domain.Service;
using BudgetTracker.Core.Domain.UnitOfWork;
using BudgetTracker.Core.Infrastructure.Repository;
using BudgetTracker.Core.Infrastructure.Services;


namespace BudgetTracker.Application.Service
{
    public class StockAppService(IStockRepository stockRepository, IStockMarketService stockMarketService, IUnitOfWork unitOfWork) : IStockAppService
    {
        private readonly IStockRepository _stockRepository = stockRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        private readonly IStockMarketService _stockMarketService = stockMarketService;
        public async Task CreateAsync(long accountId, CreateStockRequest request)
        {
            var stock = await _stockRepository.GetByTickerAsync(accountId, request.Ticker);

            if (stock is not null)
            {
                throw new InvalidOperationException("This stock already exists!");
            }

            var newStock = new Stock()
            {
                AccountId = accountId,
                Ticker = request.Ticker,
                Title = request.Title,
                Quantity = request.Quantity,
                Description = request.Description,
                PriceBuyed = request.Price,
            };

            await _stockRepository.AddAsync(newStock);
        }

        public async Task<IPagedResult<GetAllStockResponse>> GetAllStockAsync(long accountId, int page = 1)
        {
            var stocks = (await _stockRepository.GetAllAsync(accountId)).ToList();

            var tickers = stocks.Select(s => s.Ticker).ToList();

            var liveStocks = await _stockMarketService.GetStockByTickerAsync(tickers);

            var allStocks = stocks.Select(x =>
            {
                var live = liveStocks.FirstOrDefault(l => l.Ticker == x.Ticker);
                var percentage = stocks is not null && x.PriceBuyed > 0
                        ? (live.PriceMarket - x.PriceBuyed) / x.PriceBuyed * 100
                        : 0;
                return new GetAllStockResponse()
                {
                    Ticker = x.Ticker,
                    PriceBuyed = x.PriceBuyed,
                    PriceMarket = live?.PriceMarket ?? 0,
                    Percentage = $"{Math.Round(percentage, 2)}%"
                };

            }).ToList();


            foreach (var live in liveStocks)
            {
                _unitOfWork.BeginTransaction();

                var data = stocks.FirstOrDefault(i => i.Ticker == live.Ticker) ?? null;
                var percentage = data is not null && data.PriceBuyed > 0
                        ? (live.PriceMarket - data.PriceBuyed) / data.PriceBuyed * 100
                        : 0;

                Stock stock = new()
                {
                    Id = data.Id,
                    UpdatedAt = DateTime.UtcNow,
                    PriceBuyed = data.PriceBuyed,
                    Quantity = data.Quantity,
                    PriceMarket = live.PriceMarket,
                    AccountId = data.AccountId,
                    Avarage = $"{Math.Round(percentage, 2)}%",
                    Ticker = data.Ticker,
                    Title = data.Title
                };
                await _stockRepository.UpdateAsync(stock);
                _unitOfWork.Commit();
            }


            var pageSize = 10;

            var paged = allStocks
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new IPagedResult<GetAllStockResponse>()
            {
                Items = paged,
                PageNumber = page,
                PageSize = pageSize,
                TotalRecords = allStocks.Count,
            };
        }
    }
}
