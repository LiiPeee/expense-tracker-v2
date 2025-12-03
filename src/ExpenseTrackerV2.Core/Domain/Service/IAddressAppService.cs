using ExpenseTrackerV2.Application.Dtos.Request;

namespace ExpenseTrackerV2.Core.Domain.Service
{
    public interface IAddressAppService
    {

        Task CreateAsync(AddressRequest request);
    }
}
