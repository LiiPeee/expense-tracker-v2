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
    private readonly IUnitOfWork _unitOfWork;
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
            await _unitOfWork.BeginTransactionAsync();

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

            await _unitOfWork.CommitAsync();

            return savedContact;
        }
        catch (Exception ex) 
        { 
            await _unitOfWork.RollbackAsync();

            throw ex;
        }
       

    }
}
