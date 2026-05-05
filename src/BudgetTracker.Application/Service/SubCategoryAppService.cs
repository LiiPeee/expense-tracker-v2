using BudgetTracker.Core.Domain.Entities;
using BudgetTracker.Core.Domain.Models.Request;
using BudgetTracker.Core.Domain.Service;
using BudgetTracker.Core.Domain.UnitOfWork;
using BudgetTracker.Core.Infrastructure.Repository;

namespace BudgetTracker.Application.Service
{
    public class SubCategoryAppService : ISubCategoryAppService
    {
        public readonly ISubCategoryRepository _subCategoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public SubCategoryAppService(ISubCategoryRepository subCategoryRepository, IUnitOfWork unitOfWork)
        {
            _subCategoryRepository = subCategoryRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task CreateAsync(long accountId, CreateSubCategoryRequest request)
        {
            SubCategory subCategory = new SubCategory
            {
                Name = request.Name,
                CategoryId = request.CategoryId,
                Description = request.Description,
                IsActive = request.IsActive,
                AccountId = accountId
            };

            await _subCategoryRepository.AddAsync(subCategory);

            _unitOfWork.Commit();
        }

        public async Task<IEnumerable<SubCategory>> GetAllAsync()
        {
            return await _subCategoryRepository.GetAllAsync();
        }
    }
}


