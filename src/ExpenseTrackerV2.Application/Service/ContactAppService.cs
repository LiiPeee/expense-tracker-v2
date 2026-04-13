using ExpenseTrackerV2.Application.Dtos.Request;
using ExpenseTrackerV2.Core.Domain.Entities;
using ExpenseTrackerV2.Core.Domain.Repository;
using ExpenseTrackerV2.Core.Domain.Service;
using ExpenseTrackerV2.Core.Domain.UnitOfWork;

namespace ExpenseTrackerV2.Application.Service;

public class ContactAppService : IContactAppService
{
    private readonly IContactRepository _contactRepository;
    private readonly IAddressRepository _addressRepository;
    public readonly IUnitOfWork _unitOfWork;

    public ContactAppService(IContactRepository contactRepository, IAddressRepository addressRepository, IUnitOfWork unitOfWork)
    {
        _contactRepository = contactRepository;
        _addressRepository = addressRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Contact?> CreateAsync(long accountId, ContactRequest request)
    {
        try
        {
            Address address = new()
            {
                City = request.City,
                Country = request.Country,
                CreatedAt = DateTime.UtcNow,
                IsPrimary = request.IsPrimary,
                State = request.State,
                Street = request.Street,
                ZipCode = request.ZipCode,
            };

            var typeContactId = EnumHelper.GetId(request.TypeContact);

            var savedAddress = await _addressRepository.AddAsync(address);

            List<Address> listAddress = new List<Address>()
            {
                savedAddress,
            };

            Contact contact = new Contact()
            {
                CreatedAt = DateTime.UtcNow,
                Document = request.Document,
                Email = request.Email,
                IsActive = true,
                Name = request.Name,
                Phone = request.Phone,
                AccountId = accountId,
                TypeContactId = typeContactId,
                Address = listAddress
            };

            var savedContact = await _contactRepository.AddAsync(contact);

            _unitOfWork.Commit();

            return savedContact;
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task<List<Contact?>> GetAllsync(long accountId)
    {
        try
        {
            var contact = await _contactRepository.GetByIdAccount(accountId);
            return contact;
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task EditContactAsync(long accountId, ContactRequest request)
    {
        try
        {
            if (!long.TryParse(request.ContactId, out var id))
                throw new ArgumentException("ContactId inválido");
            var existingContact = await _contactRepository.GetByIdAsync(id);

            if (existingContact == null || existingContact.AccountId != accountId)
                throw new UnauthorizedAccessException("Contact not found or access denied");

            Contact contact = new Contact()
            {
                Id = id,
                AccountId = accountId,
                Document = request.Document,
                Email = request.Email,
                Name = request.Name,
                Phone = request.Phone,
                TypeContactId = EnumHelper.GetId(request.TypeContact),
                UpdatedAt = DateTime.UtcNow,
            };

            await _contactRepository.UpdateAsync(contact);
            _unitOfWork.Commit();

        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task DeleteContactAsync(long accountId, string contactId)
    {
        try
        {
            if (!long.TryParse(contactId, out var id))
                throw new ArgumentException("ContactId inválido");

            var contact = await _contactRepository.GetByIdAsync(id);
            if (contact == null || contact.AccountId != accountId)
                throw new UnauthorizedAccessException("Contact not found or access denied");

            await _contactRepository.DeleteAsync(id);
            _unitOfWork.Commit();

        }
        catch (Exception ex)
        {
            _unitOfWork.Rollback();
            throw;
        }
    }
}
