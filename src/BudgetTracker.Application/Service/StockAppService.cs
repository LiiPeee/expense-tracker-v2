using BudgetTracker.Core.Domain.Dtos.Output;
using BudgetTracker.Core.Domain.Entities;
using BudgetTracker.Core.Domain.Models.Request;
using BudgetTracker.Core.Domain.Service;
using BudgetTracker.Core.Infrastructure.Repository;
using BudgetTracker.Core.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetTracker.Application.Service
{
    public class StockAppService(IStockRepository stockRepository, IStockMarketService stockMarketService) : IStockAppService
    {
        private readonly IStockRepository _stockRepository = stockRepository;
        private readonly IStockMarketService _stockMarketService = stockMarketService;
        public async Task CreateAsync(long accountId, CreateStockRequest request)
        {
            var stock = await _stockRepository.GetByTickerAsync(accountId, request.Ticker);

            if(stock is not null)
            {
               throw new UnauthorizedAccessException("This stock already exists!");
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

        public async Task<IPagedResult<Stock>> GetAllStockAsync(long accountId)
        {
            var stocks = (await _stockRepository.GetAllAsync(accountId)).ToList();

            var tickers = stocks.Select(s => s.Ticker).ToList();

            // TODO(Stock WIP): enrich with live quotes once IStockMarketService is implemented.
            var liveStocks = await _stockMarketService.GetStockByTickerAsync(tickers);

            return new IPagedResult<Stock>()
            {
                Items = stocks,
                PageNumber = 1,
                PageSize = 10,
                TotalRecords = stocks.Count
            };
        }
    }
}
