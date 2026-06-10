using BudgetTracker.Application.Dtos.Request;
using BudgetTracker.Core.Domain.Entities;
using BudgetTracker.Core.Domain.Repository;
using BudgetTracker.Core.Domain.Service;
using BudgetTracker.Core.Domain.UnitOfWork;

namespace BudgetTracker.Application.Service;

public class AddressAppService(IContactRepository contactRepository, IAddressRepository addressRepository, IUnitOfWork unitOfWork) : IAddressAppService
{
    private readonly IAddressRepository _addressRepository = addressRepository;
    public readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task CreateAsync(AddressRequest addressRequest)
    {
        try
        {
            Address address = new Address()
            {
                CreatedAt = DateTime.UtcNow,
                City = addressRequest.City,
                Country = addressRequest.Country,
                IsPrimary = addressRequest.IsPrimary,
                State = addressRequest.State,
                Street = addressRequest.Street,
                ZipCode = addressRequest.ZipCode,
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


