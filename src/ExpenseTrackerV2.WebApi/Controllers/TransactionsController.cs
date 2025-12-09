using ExpenseTrackerV2.Application.Dtos.Request;
using ExpenseTrackerV2.Core.Domain.Entities;
using ExpenseTrackerV2.Core.Domain.Service;
using Microsoft.AspNetCore.Http;
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
        public async Task<Transactions> CreateAsync(TransactionRequest transactionRequest)
        {
            return await _transactionAppService.CreateAsync(transactionRequest);
        }

    }
}