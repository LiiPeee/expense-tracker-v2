
using ExpenseTrackerV2.Application.Dtos.Request;
using ExpenseTrackerV2.Core.Domain.Entities;
using ExpenseTrackerV2.Core.Domain.Enum;
using ExpenseTrackerV2.Core.Domain.Repository;
using ExpenseTrackerV2.Core.Domain.Service;
namespace ExpenseTrackerV2.Application.Service;

public class TransactionAppService : ITransactionAppService
{
    private readonly ITransactionsRepository _transactionRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IContactRepository _contactRepository;

    public TransactionAppService(ITransactionsRepository transactionRepository, ICategoryRepository categoryRepository, IContactRepository contactRepository)
    {
        _transactionRepository = transactionRepository;
        _categoryRepository = categoryRepository;
        _contactRepository = contactRepository;
    }
    public async Task<Transactions?> CreateAsync(TransactionRequest transactionRequest)
    {
        var contact = await _contactRepository.GetByNameAsync(transactionRequest.ContactName);

        var category = await _categoryRepository.GetByNameAsync(transactionRequest.CategoryName);


        if (contact is null || category is null) 
        {
            throw new Exception("failed transaction");
        }

        if(transactionRequest.NumberOfInstallment > 0)
        {
            return await CreateInstallemntsAsync(transactionRequest, category, contact);
        }

        Transactions transactions = new()
        {
            AccountId = 1,
            Amount = transactionRequest.Amount,
            CategoryId = category.Id,
            Contact = contact,
            NumberOfInstallment = null,
            Paid = false,
            Recurrence = transactionRequest.Recurrence,
            TypeTransaction = transactionRequest.TypeTransaction,
            Description = transactionRequest.Description,
            DateOfInstallment = null,
        };

        return await _transactionRepository.AddAsync(transactions);
    }

    private async Task<Transactions?> CreateInstallemntsAsync(TransactionRequest request, Category category, Contact contact)
    {
        if(request.NumberOfInstallment is not null)
        {
            var dateNow = DateTime.UtcNow;

            for (var i = 0; i < request.NumberOfInstallment;i++)
            {
                var dateInstallemnts = new DateOnly(dateNow.Year, dateNow.Month, (int)request.DateOfInstallment).AddMonths(i);

                Transactions transaction = new()
                {
                    AccountId = 1,
                    Category = category,
                    Contact = contact,
                    DateOfInstallment = dateInstallemnts,
                    Description = request.Description,
                    NumberOfInstallment = request.NumberOfInstallment,
                    Paid = false,
                    Recurrence = Recurrence.Monthly,
                    TypeTransaction = request.TypeTransaction,
                };

               return await _transactionRepository.AddAsync(transaction);
            }
        }
        return null;
    }
}
