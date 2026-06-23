using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BudgetTracker.Core.Domain.Dtos.Output;
using BudgetTracker.Core.Domain.Entities;
using BudgetTracker.Core.Domain.Models.Request.BudgetLimit;
using BudgetTracker.Core.Domain.Repository;
using BudgetTracker.Core.Domain.Service;
using BudgetTracker.Core.Domain.UnitOfWork;
using BudgetTracker.Core.Infrastructure.Repository;

namespace BudgetTracker.Application.Service
{
    public class BudgetLimitService(IBudgetLimitRepository budgetLimitRepository, IUnitOfWork unitOfWork, ICategoryRepository categoryRepository, ITransactionsRepository transactionRepository) : IBudgetLimitService
    {
        private readonly IBudgetLimitRepository _budgetLimitRepository = budgetLimitRepository;
        private readonly ICategoryRepository _categoryRepository = categoryRepository;
        private readonly ITransactionsRepository _transactionRepository = transactionRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;


        public async Task<BudgetLimit> CreateAsync(CreateBudgetLimit request)
        {
            try
            {
                if (request.LimitAmount <= 0)
                    throw new ArgumentException("limit amount must be greater than zero");

                if (request.Month is < 1 or > 12)
                    throw new ArgumentException("month must be between 1 and 12");

                if (request.Year is < 2000 or > 2100)
                    throw new ArgumentException("year is invalid");

                var category = await _categoryRepository.GetByIdAsync(EnumHelper.Category(request.CategoryName)) ?? throw new KeyNotFoundException("we cannot find category for this transaction");

                BudgetLimit budget = new()
                {
                    AccountId = request.AccountId,
                    CategoryId = category.Id,
                    LimitAmount = request.LimitAmount,
                    IsLimit = false,
                    Month = request.Month,
                    Year = request.Year
                };

                await _budgetLimitRepository.AddAsync(budget);

                return budget;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                throw;
            }
        }

        public async Task<IPagedResult<BudgetLimit?>> GetByAccountIdAsync(long accountId, int pageNumber = 1)
        {
            var result = await _budgetLimitRepository.GetByAccountIdAsync(accountId, pageNumber);

            // Spending is computed on read from the transactions themselves — the configured
            // LimitAmount is never mutated, so there is no denormalized counter to drift.
            foreach (var budget in result.Items)
            {
                if (budget is null) continue;

                var spent = await _transactionRepository.GetExpenseTotalByCategoryAsync(
                    accountId, budget.CategoryId, budget.Month, budget.Year);

                budget.Percentage = budget.LimitAmount > 0
                    ? Math.Round(spent / budget.LimitAmount * 100, 2)
                    : 0;
                budget.IsLimit = spent > budget.LimitAmount;
            }

            return result;
        }
    }
}
