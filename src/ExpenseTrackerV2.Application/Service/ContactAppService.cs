using ExpenseTrackerV2.Application.Dtos.Request;
using ExpenseTrackerV2.Core.Domain.Entities;
using ExpenseTrackerV2.Core.Domain.Repository;
using ExpenseTrackerV2.Core.Domain.Service;
using System;

namespace ExpenseTrackerV2.Application.Service;

public class ContactAppService : IContactAppService
{
    private readonly IContactRepository _contactRepository;

    public ContactAppService(IContactRepository contactRepository)
    {
        _contactRepository = contactRepository;
    }
    public async Task CreateAsync(ContactRequest request)
    {
        Contact contact = new Contact()
        {
            CreatedAt = DateTime.Now,
            Document = request.Document,
            Email = request.Email,
            IsActive = true,
            Name = request.Name,
            Phone = request.Phone,
            Type = request.TypeContact,
            
        };
        await _contactRepository.AddAsync(contact);
    }
}
