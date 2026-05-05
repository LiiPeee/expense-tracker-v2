using BudgetTracker.Core.Domain.Service;
using Microsoft.AspNetCore.Mvc;

namespace BudgetTracker.WebApi.Controller
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

