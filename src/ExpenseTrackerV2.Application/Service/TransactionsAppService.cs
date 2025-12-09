
using ExpenseTrackerV2.Application.Dtos.Request;
using ExpenseTrackerV2.Core.Domain.Entities;
using ExpenseTrackerV2.Core.Domain.Repository;
using ExpenseTrackerV2.Core.Domain.Service;
using ExpenseTrackerV2.Core.Domain.UnitOfWork;
namespace ExpenseTrackerV2.Application.Service;

public class TransactionsAppService : ITransactionsAppService
{
    private readonly ITransactionsRepository _transactionRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IContactRepository _contactRepository;
    private readonly IUnitOfWork _unitOfWork;

    public TransactionsAppService(ITransactionsRepository transactionRepository, ICategoryRepository categoryRepository, IContactRepository contactRepository, IUnitOfWork unitOfWork)
    {
        _transactionRepository = transactionRepository;
        _categoryRepository = categoryRepository;
        _contactRepository = contactRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task<Transactions?> CreateAsync(TransactionRequest transactionRequest)
    {
        await _unitOfWork.BeginTransactionAsync();
       
        var contact = await _contactRepository.GetByNameAsync(transactionRequest.ContactName);

        var category = await _categoryRepository.GetByNameAsync(transactionRequest.CategoryName);

        if (category is null) 
        {
            throw new Exception("failed transaction");
        }

        var recurrenceId = 1;

        var typeTransactionId = 1;

        if (transactionRequest.NumberOfInstallment > 0)
        {
            return await CreateInstallemntsAsync(transactionRequest, category, contact, recurrenceId, typeTransactionId);
        }
       
        Transactions transactions = new()
        {
            AccountId = 1,
            Name = transactionRequest.TransactionName,
            Amount = transactionRequest.Amount,
            CategoryId = category.Id,
            NumberOfInstallment = null,
            Paid = false,
            RecurrenceId = recurrenceId,
            TypeTransactionId = typeTransactionId,
            Description = transactionRequest.Description,
            DateOfInstallment = null,
        };

        return await _transactionRepository.AddAsync(transactions);
    }

    private async Task<Transactions?> CreateInstallemntsAsync(TransactionRequest request, Category category, long contactId, long recurrenceId, long typeTransactionId)
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
                    Amount = request.Amount,
                    Name = request.TransactionName,
                    Category = category,
                    ContactId = contactId,
                    DateOfInstallment = dateInstallemnts,
                    Description = request.Description,
                    NumberOfInstallment = request.NumberOfInstallment,
                    Paid = false,
                    RecurrenceId = recurrenceId,
                    TypeTransactionId = typeTransactionId,
                };

               return await _transactionRepository.AddAsync(transaction);
            }
        }
        return null;
    }
}
