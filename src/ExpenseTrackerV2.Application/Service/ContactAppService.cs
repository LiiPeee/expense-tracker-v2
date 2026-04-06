using ExpenseTrackerV2.Application.Dtos.Request;
using ExpenseTrackerV2.Core.Domain.Entities;
using ExpenseTrackerV2.Core.Domain.Repository;
using ExpenseTrackerV2.Core.Domain.Service;
using ExpenseTrackerV2.Core.Domain.UnitOfWork;
using ExpenseTrackerV2.Core.Infrastructure.Repository;
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
    public async Task<Contact?> CreateAsync(ContactRequest request)
    {
        try
        {
            var accountId = long.Parse(Environment.GetEnvironmentVariable("ACCOUNT_ID"));

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
                CreatedAt = DateTime.Now,
                Document = request.Document,
                Email = request.Email,
                IsActive = true,
                Name = request.Name,
                Phone = request.Phone,
                AccountId = 2,
                TypeContactId = typeContactId,
                Address = listAddress
            };

            var savedContact = await _contactRepository.AddAsync(contact);

            _unitOfWork.Commit();


            return savedContact;
        }
        catch (Exception ex) 
        { 
            throw ex;
        }
    }
    public async Task<List<Contact?>> GetAllsync()
    {
        try
        {
            var accountId = long.Parse(Environment.GetEnvironmentVariable("ACCOUNT_ID"));

            var contact = await _contactRepository.GetByIdAccount(accountId);
            return contact;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public async Task EditContactAsync(ContactRequest request)
    {
        try
        {
            Contact contact = new Contact()
            {
                Id = long.Parse(request.ContactId),
                Document = request.Document,
                Email = request.Email,
                Name = request.Name,
                Phone = request.Phone,
                TypeContactId = EnumHelper.GetId(request.TypeContact),
                UpdatedAt = DateTime.UtcNow,
            };

            await _contactRepository.UpdateAsync(contact);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public async Task DeleteContactAsync(string contactId)
    {
        try
        {
            var id = long.Parse(contactId);
            await _contactRepository.DeleteAsync(id);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}
