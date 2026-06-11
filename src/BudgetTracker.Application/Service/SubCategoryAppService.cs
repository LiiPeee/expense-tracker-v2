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
            try
            {
                if (await _subCategoryRepository.GetByNameAsync(accountId, request.Name, request.CategoryId ) is not null)
                    throw new ArgumentException("subcategory already exists for this account");

                SubCategory subCategory = new SubCategory
                {
                    Name = request.Name,
                    CategoryId = request.CategoryId,
                    Description = request.Description,
                    IsActive = request.IsActive,
                    AccountId = accountId
                };

                _unitOfWork.BeginTransaction();
                await _subCategoryRepository.AddAsync(subCategory);
                _unitOfWork.Commit();
            }
            catch
            {
                _unitOfWork.Rollback();
                throw;
            }
        }

        public async Task<IEnumerable<SubCategory>> GetAllAsync()
        {
            return await _subCategoryRepository.GetAllAsync();
        }
    }
}


