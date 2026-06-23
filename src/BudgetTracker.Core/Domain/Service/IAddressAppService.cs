using BudgetTracker.Application.Dtos.Request;

namespace BudgetTracker.Core.Domain.Service
{
    public interface IAddressAppService
    {
        Task CreateAsync(long accountId, AddressRequest request);
    }
}


