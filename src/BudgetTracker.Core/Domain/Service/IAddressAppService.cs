using BudgetTracker.Core.Domain.Models.Request.Address;

namespace BudgetTracker.Core.Domain.Service
{
    public interface IAddressAppService
    {
        Task CreateAsync(long accountId, AddressRequest request);
    }
}


