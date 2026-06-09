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
    public class BudgetLimitService(IBudgetLimitRepository budgetLimitRepository, IUnitOfWork unitOfWork, ICategoryRepository categoryRepository) : IBudgetLimitService
    {
        private readonly IBudgetLimitRepository _budgetLimitRepository = budgetLimitRepository;
        private readonly ICategoryRepository _categoryRepository = categoryRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;


        public async Task<BudgetLimit> CreateAsync(CreateBudgetLimit request)
        {
            var category = await _categoryRepository.GetByNameAsync(request.CategoryName) ?? throw new KeyNotFoundException("we cannot find category for this transaction"); ;

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

        public async Task<BudgetLimit> UpdateAsync(decimal amount,string categoryName, long accountId)
        {
            BudgetLimit budgetLimit = await _budgetLimitRepository.GetByCategoryAndAccountIdAsync(categoryName, accountId) 
                ?? throw new KeyNotFoundException("we cannot find budget limit for this category");

            var exceed = budgetLimit.LimitAmount + amount > budgetLimit.LimitAmount;

            var percentage = (amount / budgetLimit.LimitAmount) * 100;

            budgetLimit.Percentage = percentage;
            budgetLimit.IsLimit = exceed;

            _unitOfWork.BeginTransaction();

            await _budgetLimitRepository.UpdateAsync(budgetLimit);
            _unitOfWork.Commit();

            return budgetLimit;
        }

        public async Task<IPagedResult<BudgetLimit?>> GetByAccountIdAsync(long accountId, int pageNumber = 1)
        {
            return await _budgetLimitRepository.GetByAccountIdAsync(accountId, pageNumber);
        }
    }
}
