using ExpenseTrackerV2.Application.Dtos.Request;
using ExpenseTrackerV2.Core.Domain.Entities;
using ExpenseTrackerV2.Core.Domain.Repository;
using ExpenseTrackerV2.Core.Domain.Service;

namespace ExpenseTrackerV2.Application.Service;

public class AccountAppService : IAccountAppService
{
    private readonly IAccountRepository _accountRepository;
    private readonly IOrganizationRepository _organizationRepository;

    public AccountAppService(IAccountRepository accountRepository, IOrganizationRepository organizationRepository)
    {
        _accountRepository = accountRepository;
        _organizationRepository = organizationRepository;
    }

    public async Task CreateAsync(AccountRequest request)
    {
        var organization = await _organizationRepository.GetByIdAsync(request.OrganizationId) ?? throw new ArgumentException("We didnt find this organization");

        var account = new Account
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            Password = request.Password,
            Balance = request.Balance,
            OrganizationId = organization.Id,
        };

        await _accountRepository.AddAsync(account);
    }


}
