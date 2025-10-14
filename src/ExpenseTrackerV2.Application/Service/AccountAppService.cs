using System;
using ExpenseTrackerV2.Application.Dtos.Request;
using ExpenseTrackerV2.Core.Domain.Entities;
using ExpenseTrackerV2.Core.Domain.Repository;

namespace ExpenseTrackerV2.Application.Service;

public class AccountAppService(IAccountRepository accountRepository, IOrganizationRepository organizationRepository)
{
    private readonly IAccountRepository _accountRepository = accountRepository;
    private readonly IOrganizationRepository _organizationRepository = organizationRepository;

    public async Task<Account> CreateAsync(AccountRequest request)
    {

        var organization = _organizationRepository.GetByIdAsync(request.OrganizationId);

        var account = new Account
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            Password = request.Password,
            Balance = request.Balance,
            OrganizationId = organization.Id,
            Organization = organization,
        };

        return await _accountRepository.AddAsync(account);
    }


}
