using ExpenseTrackerV2.Core.Domain.Service;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTrackerV2.WebApi.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        public readonly IAddressAppService _addressAppService;
        public AddressController(IAddressAppService addressAppService)
        {
            _addressAppService = addressAppService;
        }
    }
}
