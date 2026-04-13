using ExpenseTrackerV2.Core.Domain.Dtos.Output;
using ExpenseTrackerV2.Core.Domain.Dtos.Request.Transaction;
using ExpenseTrackerV2.Core.Domain.Entities;
using ExpenseTrackerV2.Core.Domain.Enum;
using ExpenseTrackerV2.Core.Domain.Repository;
using ExpenseTrackerV2.Core.Domain.Service;
using ExpenseTrackerV2.Core.Domain.UnitOfWork;
using ExpenseTrackerV2.Core.Infrastructure.Repository;

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

    public async Task<List<Transactions>> CreateAsync(long accountId, CreateTrasactionRequest transactionRequest)
    {
        try
        {
            _unitOfWork.BeginTransaction();

            var contact = await _contactRepository.GetByNameAsync(accountId, transactionRequest.ContactName);
            var category = await _categoryRepository.GetByNameAsync(EnumHelper.Category(transactionRequest.CategoryName.ToString()));

            if (category is null || contact is null)
            {
                throw new Exception("we cannot find contact or category for this transaction");
            }

            var subCategory = await _subCategoryRepository.GetByNameAsync(transactionRequest.SubCategoryName)
                ?? await _subCategoryRepository.AddAsync(new SubCategory { Name = transactionRequest.SubCategoryName, IsActive = true, CategoryId = category.Id });

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

            return new List<Transactions> { savedTransaction };
        }
        catch (Exception ex)
        {
            _unitOfWork.Rollback();
            throw;
        }
    }

    public async Task PaidAsync(long accountId, PaidTransactionRequest paidTransactionRequest)
    {
        try
        {
            _unitOfWork.BeginTransaction();

            var transaction = await _transactionRepository.GetByIdAsync(paidTransactionRequest.TransactionId);

            if (transaction == null || transaction.AccountId != accountId)
                throw new UnauthorizedAccessException("Transaction not found or access denied");

            if (transaction is not null)
            {
                var account = await _accountRepository.GetByIdAsync(transaction.AccountId)
                    ?? throw new Exception("we cannot find account");

                if (paidTransactionRequest.Paid == true && transaction.Paid != true)
                {
                    var newBalance = SumOrSubtractBalance(account.Balance, transaction.Amount, transaction.TypeTransactionId);

                    transaction.Paid = true;

                    var updatedTransaction = await _transactionRepository.UpdateAsync(transaction);

                    if (!updatedTransaction)
                    {
                        throw new Exception("something wrong happen");
                    }
                    account.Balance = newBalance;

                    await _accountRepository.UpdateAsync(account);
                    _unitOfWork.Commit();
                }
            }
        }
        catch (Exception ex)
        {
            _unitOfWork.Rollback();
            throw;
        }
    }

    public async Task DeleteAsync(long accountId, long id)
    {
        try
        {
            _unitOfWork.BeginTransaction();

            await _transactionRepository.DeleteTransactionAsync(accountId, id);

            _unitOfWork.Commit();
        }
        catch (Exception ex)
        {
            _unitOfWork.Rollback();
            throw;
        }
    }

    public async Task<List<FilterByMonthAndYearOutPut>> FilterExpenseWithContactAsync(long accountId, long year, long month)
    {
        try
        {
            var transactions = await _transactionRepository.FilterExpenseMonthWithContactAsync(accountId, year, month);
            var filter = new List<FilterByMonthAndYearOutPut>();

            foreach (var t in transactions)
            {
                var outputFilter = new FilterByMonthAndYearOutPut()
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
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task<decimal> FilterExpenseMonthAndYearAsync(long accountId, long year, long month)
    {
        try
        {
            var transactions = await _transactionRepository.FilterExpenseMonthAndYearAsync(accountId, year, month);

            decimal totalExpense = 0;

            foreach (var transaction in transactions)
            {
                totalExpense += transaction.Amount;
            }

            return totalExpense;
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task<decimal> FilterIncomeMonthAndYearAsync(long accountId, long year, long month)
    {
        try
        {
            var transactions = await _transactionRepository.FilterIncomeMonthAndYearAsync(accountId, year, month);

            decimal totalIncome = 0;

            foreach (var transaction in transactions)
            {
                totalIncome += transaction.Amount;
            }

            return totalIncome;
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task<IPagedResult<FilterByMonthAndYearOutPut>> FilterTransactionByTypeAsync(long accountId, TypeTransaction type, long month, long year)
    {
        try
        {
            var transactions = await _transactionRepository.FilterTransactionsByTypeAsync(accountId, type.ToString(), month, year);

            if (transactions.Items.Count == 0) throw new Exception("we cannot find transactions");

            var filter = new List<FilterByMonthAndYearOutPut>();

            foreach (var i in transactions.Items)
            {
                filter.Add(new FilterByMonthAndYearOutPut
                {
                    Id = i.Id,
                    Amount = i.Amount,
                    Description = i.Description,
                    Name = i.Name,
                    Paid = i.Paid,
                    CreatedDate = i.CreatedAt,
                    TypeTransaction = i.TypeTransactionId,
                    Recurrence = i.RecurrenceId,
                    Contact = new ContactOutput
                    {
                        Email = i.Contact.Email,
                        Name = i.Contact.Name,
                        Phone = i.Contact.Phone
                    },
                    Category = new CategoryOutput
                    {
                        Name = i.Category.Name,
                    },
                    QuantityOfInstallment = !string.IsNullOrEmpty(i.QuantityInstallment) ? i.QuantityInstallment : null,
                    DateOfInstallment = !string.IsNullOrEmpty(i.QuantityInstallment) ? i.DateOfInstallment : null
                });
            }

            return new IPagedResult<FilterByMonthAndYearOutPut>
            {
                PageNumber = transactions.PageNumber,
                PageSize = transactions.PageSize,
                TotalRecords = transactions.TotalRecords,
                Items = filter
            };
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task<IPagedResult<FilterByMonthAndYearOutPut>> FilterTransactionsByCategoryAsync(long accountId, Categories categoryName, TypeTransaction type, long month, long year)
    {
        try
        {
            var transactions = await _transactionRepository.FilterTransactionsByCategoryAsync(accountId, categoryName.ToString(), type.ToString(), month, year);

            if (transactions.Items.Count == 0) throw new Exception("we cannot find transactions");

            var filter = new List<FilterByMonthAndYearOutPut>();

            foreach (var i in transactions.Items)
            {
                filter.Add(new FilterByMonthAndYearOutPut
                {
                    Id = i.Id,
                    Amount = i.Amount,
                    Description = i.Description,
                    Name = i.Name,
                    Paid = i.Paid,
                    TypeTransaction = i.TypeTransactionId,
                    Recurrence = i.RecurrenceId,
                    Contact = new ContactOutput
                    {
                        Email = i.Contact.Email,
                        Name = i.Contact.Name,
                        Phone = i.Contact.Phone
                    },
                    Category = new CategoryOutput
                    {
                        Name = i.Category.Name,
                    },
                    QuantityOfInstallment = !string.IsNullOrEmpty(i.QuantityInstallment) ? i.QuantityInstallment : null,
                    DateOfInstallment = !string.IsNullOrEmpty(i.QuantityInstallment) ? i.DateOfInstallment : null
                });
            }

            return new IPagedResult<FilterByMonthAndYearOutPut>
            {
                PageNumber = transactions.PageNumber,
                PageSize = transactions.PageSize,
                TotalRecords = transactions.TotalRecords,
                Items = filter
            };
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task<IPagedResult<FilterByMonthAndYearOutPut>> FilterByMonthAndYearsync(long accountId, long month, long year, int pageNumber = 1)
    {
        try
        {
            var transactions = await _transactionRepository.FilterByMonthAndYearAsync(accountId, month, year, pageNumber);

            if (transactions.Items.Count == 0) throw new Exception("we cannot find transactions");

            var filter = new List<FilterByMonthAndYearOutPut>();

            foreach (var i in transactions.Items)
            {
                filter.Add(new FilterByMonthAndYearOutPut
                {
                    Id = i.Id,
                    Amount = i.Amount,
                    Description = i.Description,
                    Name = i.Name,
                    Paid = i.Paid,
                    TypeTransaction = i.TypeTransactionId,
                    Recurrence = i.RecurrenceId,
                    Contact = new ContactOutput
                    {
                        Email = i.Contact.Email,
                        Name = i.Contact.Name,
                        Phone = i.Contact.Phone
                    },
                    Category = new CategoryOutput
                    {
                        Name = i.Category.Name,
                    },
                    QuantityOfInstallment = !string.IsNullOrEmpty(i.QuantityInstallment) ? i.QuantityInstallment : null,
                    DateOfInstallment = !string.IsNullOrEmpty(i.QuantityInstallment) ? i.DateOfInstallment : null
                });
            }

            return new IPagedResult<FilterByMonthAndYearOutPut>
            {
                PageNumber = transactions.PageNumber,
                PageSize = transactions.PageSize,
                TotalRecords = transactions.TotalRecords,
                Items = filter
            };
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task<List<FilterByMonthAndYearOutPut>> FilterByContactAndMonth(long accountId, long year, long month, TypeTransaction type, string contactName)
    {

        var transactions = await _transactionRepository.FilterByMonthAndContactAsync(accountId, year, month, type.ToString(), contactName);

        var filter = new List<FilterByMonthAndYearOutPut>();

        foreach (var t in transactions)
        {
            var outputFilter = new FilterByMonthAndYearOutPut()
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

    public async Task<decimal> GetEconomyAsync(long accountId, long year, long month)
    {
        try
        {
            var expense = await _transactionRepository.FilterExpenseMonthAndYearAsync(accountId, year, month);
            var income = await _transactionRepository.FilterIncomeMonthAndYearAsync(accountId, year, month);

            var totalExpense = 0m;
            var totalIncome = 0m;

            foreach (var e in expense)
            {
                totalExpense += e.Amount;
            }

            foreach (var i in income)
            {
                totalIncome += i.Amount;
            }

            var total = totalIncome - totalExpense;

            return total;
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    private async Task<List<Transactions>> CreateInstallemntsAsync(CreateTrasactionRequest request, long category, long contactId, long recurrenceId, long typeTransactionId, long accountId)
    {
        try
        {
            var dateNow = DateTime.UtcNow;
            List<Transactions> transactions = new List<Transactions>();

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
                transactions.Add(savedTransaction);
            }

            _unitOfWork.Commit();

            return transactions;
        }
        catch (Exception ex)
        {
            _unitOfWork.Rollback();
            throw;
        }
    }

    private decimal SumOrSubtractBalance(decimal balance, decimal transactionAmount, long typeTransaction)
    {
        var sum = balance + transactionAmount;
        var subtraction = balance - transactionAmount;

        return (typeTransaction == 1) ? subtraction : sum;
    }
}
