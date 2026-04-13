using ExpenseTrackerV2.Application.Dtos.Request;
using ExpenseTrackerV2.Core.Domain.Entities;

namespace ExpenseTrackerV2.Core.Domain.Service;

public interface IContactAppService
{
    Task<Contact?> CreateAsync(long accountId, ContactRequest request);
    Task<List<Contact?>> GetAllsync(long accountId);
    Task EditContactAsync(long accountId, ContactRequest request);
    Task DeleteContactAsync(long accountId, string contactId);
}
