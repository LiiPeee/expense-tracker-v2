using ExpenseTrackerV2.Core.Domain.Dtos.Output;
using ExpenseTrackerV2.Core.Domain.Dtos.Request.Transaction;
using ExpenseTrackerV2.Core.Domain.Entities;
using ExpenseTrackerV2.Core.Domain.Service;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTrackerV2.WebApi.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionsAppService _transactionAppService;
        public TransactionController(ITransactionsAppService transactionAppService)
        {
            _transactionAppService = transactionAppService;
        }

        [HttpPost("[action]")]
        public async Task<List<Transactions>> CreateAsync([FromBody] CreateTrasactionRequest transactionRequest)
        {
            return await _transactionAppService.CreateAsync(transactionRequest);
        }

        [HttpPatch("[action]")]
        public async Task EditAsync([FromBody] PaidTransactionRequest paidTransactionRequest)
        {
            await _transactionAppService.PaidAsync(paidTransactionRequest);
        }

        [HttpGet("[action]")]
        public async Task<IPagedResult<FilterByMonthAndYearOutPut>> GetByCategoryAsync([FromQuery] string categoryName, [FromQuery] long month, [FromQuery] long year, [FromQuery] string type)
        {
            return await _transactionAppService.FilterTransactionsByCategoryAsync(categoryName, type, month, year);
        }

        [HttpGet("[action]")]
        public async Task<IPagedResult<FilterByMonthAndYearOutPut>> GetByTypeAsync([FromQuery] long month, [FromQuery] long year, [FromQuery] string type)
        {
            return await _transactionAppService.FilterTransactionByTypeAsync(type, month, year);
        }

        [HttpGet("[action]")]
        public async Task<IPagedResult<FilterByMonthAndYearOutPut>> GetByMonthAndYearAsync([FromQuery] long month, [FromQuery] long year, [FromQuery] int pageNumber = 1)
        {
            return await _transactionAppService.FilterByMonthAndYearsync(month, year, pageNumber);
        }

        [HttpGet("[action]")]
        public async Task<List<FilterByMonthAndYearOutPut>> GetByContactAsync([FromQuery] long year, [FromQuery] long month, [FromQuery] string contactName, [FromQuery] string type)
        {
            return await _transactionAppService.FilterByContactAndMonth(year, month, type, contactName);
        }

        [HttpGet("[action]")]
        public async Task<decimal> GetExpenseByMonthAndYearAsync([FromQuery] long year, [FromQuery] long month)
        {
            return await _transactionAppService.FilterExpenseMonthAndYearAsync(year, month);
        }

        [HttpGet("[action]")]
        public async Task<decimal> GetIncomeByMonthAndYearAsync([FromQuery] long year, [FromQuery] long month)
        {
            return await _transactionAppService.FilterIncomeMonthAndYearAsync(year, month);
        }

        [HttpGet("[action]")]
        public async Task<decimal> GetEconomyAsync([FromQuery] long year, [FromQuery] long month)
        {
            return await _transactionAppService.GetEconomyAsync(year, month);
        }

        //[HttpGet("[action]")]
        //public async Task<List<FilterByMonthAndYearOutPut>> GetAllTransactionAsync([FromQuery] long year, [FromQuery] long month)
        //{
        //    return await _transactionAppService.GetAllTransactionsAsync(year, month);
        //}

        [HttpDelete("[action]")]
        public async Task DeleteTransactionAsync([FromQuery] long id)
        {
            await _transactionAppService.DeleteAsync(id);
        }
    }
}