using ExpenseTrackerV2.Core.Domain.Dtos.Output;
using ExpenseTrackerV2.Core.Domain.Dtos.Request.Transaction;
using ExpenseTrackerV2.Core.Domain.Entities;
using ExpenseTrackerV2.Core.Domain.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Transactions;

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
        public async Task PaidAsync([FromBody] PaidTransactionRequest paidTransactionRequest)
        {
            await _transactionAppService.PaidAsync(paidTransactionRequest);
        }

        [HttpGet("[action]")]
        public async Task<List<FilterByMonthAndCategoryOutPut>> GetByCategoryAsync([FromQuery] long categoryId, [FromQuery] long month)
        {
           return await _transactionAppService.FilterTransactionsByCategoryAsync(categoryId, month);
        }

        [HttpGet("[action]")]
        public async Task<List<FilterByMonthOutPut>> GetByMonthAndYearAsync([FromQuery] long month, [FromQuery] long year)
        {
            return await _transactionAppService.FilterByMonthAndYearsync(month,year);
        }

        [HttpGet("[action]")]
        public async Task<List<FilterByContactAndMonthOutPut>> GetByContactAndMonth([FromQuery] long month, [FromQuery] long contactId)
        {
            return await _transactionAppService.FilterByContactAndMonth(month, contactId);
        }

    }
}