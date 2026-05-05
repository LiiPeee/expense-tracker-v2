using BudgetTracker.Application.Dtos.Request;

namespace BudgetTracker.Core.Domain.Service
{
    public interface IAddressAppService
    {
        Task CreateAsync(AddressRequest request);
    }
}


