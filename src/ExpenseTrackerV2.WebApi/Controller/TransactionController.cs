using ExpenseTrackerV2.Application.Dtos.Request;
using ExpenseTrackerV2.Core.Domain.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTrackerV2.WebApi.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionAppService _transactionAppService;
        public TransactionController(ITransactionAppService transactionAppService) 
        { 
           _transactionAppService = transactionAppService;
        }

        [HttpPost]        
        public async Task CreateAsync(TransactionRequest transactionRequest)
        {
            await _transactionAppService.CreateAsync(transactionRequest);
        }
        
    }
}   
