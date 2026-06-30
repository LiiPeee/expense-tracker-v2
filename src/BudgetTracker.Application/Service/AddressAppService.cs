using BudgetTracker.Application.Dtos.Request;
using BudgetTracker.Core.Domain.Entities;
using BudgetTracker.Core.Domain.Repository;
using BudgetTracker.Core.Domain.Service;
using BudgetTracker.Core.Domain.UnitOfWork;

namespace BudgetTracker.Application.Service;

public class AddressAppService(IContactRepository contactRepository, IAddressRepository addressRepository, IUnitOfWork unitOfWork) : IAddressAppService
{
    private readonly IContactRepository _contactRepository = contactRepository;
    private readonly IAddressRepository _addressRepository = addressRepository;
    public readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task CreateAsync(long accountId, AddressRequest addressRequest)
    {
        try
        {
            var contact = await _contactRepository.GetByNameAsync(accountId, addressRequest.ContactName)
                ?? throw new UnauthorizedAccessException("Contact not found or access denied");

            Address address = new Address()
            {
                CreatedAt = DateTime.UtcNow,
                City = addressRequest.City,
                Country = addressRequest.Country,
                IsPrimary = addressRequest.IsPrimary,
                State = addressRequest.State,
                Street = addressRequest.Street,
                ZipCode = addressRequest.ZipCode,
                ContactId = contact.Id,
            };

            _unitOfWork.BeginTransaction();
            await _addressRepository.AddAsync(address);
            _unitOfWork.Commit();
        }
        catch
        {
            _unitOfWork.Rollback();
            throw;
        }
    }
}


