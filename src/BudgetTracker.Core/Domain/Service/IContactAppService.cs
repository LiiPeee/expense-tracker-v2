using BudgetTracker.Application.Dtos.Request;
using BudgetTracker.Core.Domain.Entities;
using BudgetTracker.Core.Domain.Models.Request.Transaction;

namespace BudgetTracker.Core.Domain.Service;

public interface IContactAppService
{
    Task<Contact?> CreateAsync(long accountId, CreateContactRequest request);
    Task<List<Contact?>> GetAllsync(long accountId);
    Task EditContactAsync(long accountId, ContactRequest request);
    Task DeleteContactAsync(long accountId, string contactId);
}


