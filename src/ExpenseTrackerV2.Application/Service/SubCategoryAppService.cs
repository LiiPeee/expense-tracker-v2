using ExpenseTrackerV2.Core.Domain.Entities;
using ExpenseTrackerV2.Core.Domain.Models.Request;
using ExpenseTrackerV2.Core.Domain.Service;
using ExpenseTrackerV2.Core.Domain.UnitOfWork;
using ExpenseTrackerV2.Core.Infrastructure.Repository;

namespace ExpenseTrackerV2.Application.Service
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
