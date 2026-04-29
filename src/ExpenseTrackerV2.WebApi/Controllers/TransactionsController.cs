using ExpenseTrackerV2.Core.Domain.Dtos.Output;
using ExpenseTrackerV2.Core.Domain.Dtos.Request.Transaction;
using ExpenseTrackerV2.Core.Domain.Entities;
using ExpenseTrackerV2.Core.Domain.Enum;
using ExpenseTrackerV2.Core.Domain.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace ExpenseTrackerV2.WebApi.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "User")]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionsAppService _transactionAppService;

        public TransactionController(ITransactionsAppService transactionAppService)
        {
            _transactionAppService = transactionAppService;
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<List<Transactions>>> CreateAsync([FromBody] CreateTrasactionRequest transactionRequest)
        {
            var accountId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            return Ok(await _transactionAppService.CreateAsync(accountId, transactionRequest));
        }

        [HttpPatch("[action]")]
        public async Task<ActionResult> EditAsync([FromBody] PaidTransactionRequest paidTransactionRequest)
        {
            var accountId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            await _transactionAppService.PaidAsync(accountId, paidTransactionRequest);
            return Ok();
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<IPagedResult<FilterByMonthAndYearOutPut>>> GetByCategoryAsync(
            [FromQuery] Categories categoryName,
            [FromQuery][Range(1, 12)] long month,
            [FromQuery][Range(2000, 2100)] long year,
            [FromQuery] TypeTransaction type)
        {
            var accountId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            return Ok(await _transactionAppService.FilterTransactionsByCategoryAsync(accountId, categoryName, type, month, year));
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<IPagedResult<FilterByMonthAndYearOutPut>>> GetByTypeAsync(
            [FromQuery][Range(1, 12)] long month,
            [FromQuery][Range(2000, 2100)] long year,
            [FromQuery] TypeTransaction type)
        {
            var accountId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            return Ok(await _transactionAppService.FilterTransactionByTypeAsync(accountId, type, month, year));
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<IPagedResult<FilterByMonthAndYearOutPut>>> GetByMonthAndYearAsync(
            [FromQuery][Range(1, 12)] long month,
            [FromQuery][Range(2000, 2100)] long year,
            [FromQuery][Range(1, 10)] int pageNumber = 1)
        {
            var accountId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            return Ok(await _transactionAppService.FilterByMonthAndYearsync(accountId, month, year, pageNumber));
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<List<FilterByMonthAndYearOutPut>>> GetByContactAsync(
            [FromQuery][Range(2000, 2100)] long year,
            [FromQuery][Range(1, 12)] long month,
            [FromQuery][StringLength(50)] string contactName,
            [FromQuery] TypeTransaction type, [FromQuery][Range(1, 10)] int pageNumber = 1)
        {
            var accountId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            return Ok(await _transactionAppService.FilterByContactAndMonth(accountId, year, month, type, contactName, pageNumber));
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<decimal>> GetExpenseByMonthAndYearAsync(
            [FromQuery][Range(2000, 2100)] long year,
            [FromQuery][Range(1, 12)] long month)
        {
            var accountId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            return Ok(await _transactionAppService.FilterExpenseMonthAndYearAsync(accountId, year, month));
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<decimal>> GetIncomeByMonthAndYearAsync(
            [FromQuery][Range(2000, 2100)] long year,
            [FromQuery][Range(1, 12)] long month)
        {
            var accountId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            return Ok(await _transactionAppService.FilterIncomeMonthAndYearAsync(accountId, year, month));
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<decimal>> GetEconomyAsync(
            [FromQuery][Range(2000, 2100)] long year,
            [FromQuery][Range(1, 12)] long month)
        {
            var accountId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            return Ok(await _transactionAppService.GetEconomyAsync(accountId, year, month));
        }

        [HttpDelete("[action]")]
        public async Task<ActionResult> DeleteTransactionAsync([FromQuery] long id)
        {
            var accountId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            await _transactionAppService.DeleteAsync(accountId, id);
            return Ok();
        }
    }
}