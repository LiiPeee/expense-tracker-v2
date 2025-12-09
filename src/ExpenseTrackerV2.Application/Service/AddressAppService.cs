using ExpenseTrackerV2.Application.Dtos.Request;
using ExpenseTrackerV2.Core.Domain.Entities;
using ExpenseTrackerV2.Core.Domain.Repository;
using ExpenseTrackerV2.Core.Domain.Service;
using System;

namespace ExpenseTrackerV2.Application.Service;

public class AddressAppService: IAddressAppService
{
    private readonly IAddressRepository _addressRepository;
    private readonly IContactRepository _contactRepository;

    public  AddressAppService(IContactRepository contactRepository, IAddressRepository addressRepository)
    {
        _addressRepository = addressRepository;
        _contactRepository = contactRepository;
    }

    public async Task CreateAsync(AddressRequest addressRequest)
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

        await _addressRepository.AddAsync(address);
    }


}
