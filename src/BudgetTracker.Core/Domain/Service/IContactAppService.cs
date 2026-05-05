using BudgetTracker.Application.Dtos.Request;
using BudgetTracker.Core.Domain.Entities;

namespace BudgetTracker.Core.Domain.Service;

public interface IContactAppService
{
    Task<Contact?> CreateAsync(long accountId, ContactRequest request);
    Task<List<Contact?>> GetAllsync(long accountId);
    Task EditContactAsync(long accountId, ContactRequest request);
    Task DeleteContactAsync(long accountId, string contactId);
}


