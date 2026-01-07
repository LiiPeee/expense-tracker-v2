
using Azure.Core;
using ExpenseTrackerV2.Core.Domain.Dtos.Output;
using ExpenseTrackerV2.Core.Domain.Dtos.Request.Transaction;
using ExpenseTrackerV2.Core.Domain.Entities;
using ExpenseTrackerV2.Core.Domain.Enum;
using ExpenseTrackerV2.Core.Domain.Repository;
using ExpenseTrackerV2.Core.Domain.Service;
using ExpenseTrackerV2.Core.Domain.UnitOfWork;
using ExpenseTrackerV2.Core.Infrastructure.Repository;
using System.Transactions;
namespace ExpenseTrackerV2.Application.Service;

public class TransactionsAppService : ITransactionsAppService
{
    private readonly ITransactionsRepository _transactionRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly IContactRepository _contactRepository;
    private readonly ISubCategoryRepository _subCategoryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public TransactionsAppService(ITransactionsRepository transactionRepository,
        ICategoryRepository categoryRepository,
        IContactRepository contactRepository,
        ISubCategoryRepository subCategoryRepository,
        IAccountRepository accountRepository,
        IUnitOfWork unitOfWork)
    {
        _transactionRepository = transactionRepository;
        _categoryRepository = categoryRepository;
        _contactRepository = contactRepository;
        _accountRepository = accountRepository;
        _subCategoryRepository = subCategoryRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task<List<Transactions>> CreateAsync(CreateTrasactionRequest transactionRequest)
    {
        try
        {
             _unitOfWork.BeginTransaction();

            var contact = await _contactRepository.GetByNameAsync(transactionRequest.ContactName);

            var category = await _categoryRepository.GetByNameAsync(transactionRequest.CategoryName);

            var subCategory = await _subCategoryRepository.GetByNameAsync(transactionRequest.SubCategoryName) ?? await _subCategoryRepository.AddAsync(new SubCategory { Name = transactionRequest.SubCategoryName, IsActive = true });

            if (category is null || contact is null)
            {
                throw new Exception("we cannt find contact or category for this transaction");
            }

            var accountId = long.Parse(Environment.GetEnvironmentVariable("ACCOUNT_ID"));

            var recurrenceId = EnumHelper.GetId(transactionRequest.Recurrence);

            var typeTransactionId = EnumHelper.GetId(transactionRequest.TypeTransaction);

            if (transactionRequest.NumberOfInstallment > 0)
            {
                return await CreateInstallemntsAsync(transactionRequest, category.Id, contact.Id, recurrenceId, typeTransactionId, accountId);
            }

            Transactions transaction = new()
            {
                AccountId = accountId,
                Amount = transactionRequest.Amount,
                Name = transactionRequest.TransactionName,
                CategoryId = category.Id,
                ContactId = contact.Id,
                Description = transactionRequest.Description,
                NumberOfInstallment = transactionRequest.NumberOfInstallment,
                Paid = false,
                RecurrenceId = recurrenceId,
                TypeTransactionId = typeTransactionId,
            };

            var savedTransaction = await _transactionRepository.AddAsync(transaction);

             _unitOfWork.Commit();

            List<Transactions> transactions = new List<Transactions>()
            {
                savedTransaction
            };

            return transactions;
        }
        catch (Exception ex)
        {
            _unitOfWork.Rollback();
            throw ex;
        }
    }

    public async Task PaidAsync(PaidTransactionRequest paidTransactionRequest) 
    {
        try
        {
             _unitOfWork.BeginTransaction();

            var transaction = await _transactionRepository.GetByIdAsync(paidTransactionRequest.TransactionId);

            if (transaction is not null)
            {
                var account = await _accountRepository.GetByIdAsync(transaction.AccountId) ?? throw new Exception("we cannt find account"); 
                
                if(paidTransactionRequest.Paid == true && transaction.Paid != true)
                {
                    var newBalance = SumOrSubtractBalance(account.Balance, transaction.Amount, transaction.TypeTransactionId);

                    transaction.Paid = true;

                    var updatedTransaction = await _transactionRepository.UpdateAsync(transaction);

                    if (!updatedTransaction)
                    {   
                        throw new Exception("something wrong happen");
                    }
                    account.Balance = newBalance;

                    var updatedAccount = await _accountRepository.UpdateAsync(account);

                     _unitOfWork.Commit(); 
                }
            }
        }
        catch (Exception ex) 
        {
            _unitOfWork.Rollback();
            throw ex;
        }
    }

    public async Task<decimal> FilterExpenseMonthAndYear(long year, long month)
    {
        var transactions = await _transactionRepository.FilterExpenseMonthAndYear(year, month);

        decimal totalExpense = 0;

        foreach (var transaction in transactions) 
        {
            totalExpense += transaction.Amount;
        }

        return totalExpense;
    }

    public async Task<decimal> FilterIncomeMonthAndYear(long year, long month)
    {
        var transactions = await _transactionRepository.FilterIncomeMonthAndYear(year, month);

        decimal totalIncome = 0;

        foreach (var transaction in transactions)
        {
            totalIncome += transaction.Amount;
        }

        return totalIncome;
    }

    public async Task<List<FilterByMonthAndCategoryOutPut>> FilterTransactionsByCategoryAsync(long categoryId, long month, long year)
    {
        try
        {
             _unitOfWork.BeginTransaction();

            var transaction = await _transactionRepository.FilterTransactionsByCategoryAsync(categoryId, month, year);

            var category = new List<FilterByMonthAndCategoryOutPut>();

            foreach (var t in transaction)
            {
                var filter = new FilterByMonthAndCategoryOutPut()
                {
                    Paid = t.Paid,
                    Name = t.Name,
                    Description = t.Description,
                    Amount = t.Amount,
                };

               category.Add(filter);
            }

            return category;
        }
        catch (Exception ex) 
        {
            throw ex;
        }
    }

    public async Task<List<FilterByMonthOutPut>> FilterByMonthAndYearsync(long month, long year)
    {
        try
        {
             _unitOfWork.BeginTransaction();

            var transaction = await _transactionRepository.FilterByMonthAndYearAsync(month, year);

            var category = new List<FilterByMonthOutPut>();

            foreach(var t in transaction)
            {

                var filter = new FilterByMonthOutPut()
                {
                  Amount = t.Amount,
                  Description= t.Description,
                  Name = t.Name,
                  Paid = t.Paid
                };

                category.Add(filter);
            };

            return category;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public async Task<List<FilterByContactAndMonthOutPut>> FilterByContactAndMonth(long year, long month, long contactId)
    {
        _unitOfWork.BeginTransaction();

        var transactions = await _transactionRepository.FilterByMonthAndContact(year, month, contactId);

        var filter = new List<FilterByContactAndMonthOutPut>();

        foreach(var t in transactions)
        {
            var outputFilter = new FilterByContactAndMonthOutPut()
            {
                Amount = t.Amount,
                Description = t.Description,
                Contact = new ContactOutput
                {
                    Email = t.Contact.Email,
                    Name = t.Contact.Name,
                    Phone = t.Contact.Phone
                },
                Name = t.Name,
                Paid = t.Paid
            };

            filter.Add(outputFilter);
        }

        return filter;
    }
    private async Task<List<Transactions>> CreateInstallemntsAsync(CreateTrasactionRequest request, long category, long contactId, long recurrenceId, long typeTransactionId, long accountId)
    {
        try
        {
            var dateNow = DateTime.UtcNow;
            List<Transactions> transactions = new List<Transactions> { };

            for (var i = 1; i <= request.NumberOfInstallment; i++)
            {
                var dateInstallemnts = new DateTime(dateNow.Year, dateNow.Month, (int)request.DateOfInstallment).AddMonths(i);

                Transactions transaction = new()
                {
                    AccountId = accountId,
                    Amount = request.Amount,
                    Name = request.TransactionName,
                    CategoryId = category,
                    ContactId = contactId,
                    QuantityInstallment = $"{i}/{request.NumberOfInstallment}",
                    DateOfInstallment = dateInstallemnts,
                    Description = request.Description,
                    NumberOfInstallment = request.NumberOfInstallment,
                    Paid = false,
                    RecurrenceId = recurrenceId,
                    TypeTransactionId = typeTransactionId,
                };


                var savedTransaction = await _transactionRepository.AddAsync(transaction);

                _unitOfWork.Commit();

                transactions.Add(savedTransaction);
            }

            return transactions;
        }
        catch (Exception ex) 
        {
             _unitOfWork.Rollback();

            throw ex;
        }
       
    }

    private decimal SumOrSubtractBalance(decimal balance, decimal transactionAmount, long typeTransaction)
    {
        var sum = balance + transactionAmount;
        var subtraction = balance - transactionAmount;

        return (typeTransaction == 1) ? subtraction : sum;
    }
}
