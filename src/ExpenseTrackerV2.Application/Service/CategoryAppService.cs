using ExpenseTrackerV2.Application.Dtos.Request;
using ExpenseTrackerV2.Core.Domain.Entities;
using ExpenseTrackerV2.Core.Domain.Models.Output;
using ExpenseTrackerV2.Core.Domain.Repository;
using ExpenseTrackerV2.Core.Domain.Service;
using ExpenseTrackerV2.Core.Domain.UnitOfWork;

namespace ExpenseTrackerV2.Application.Service;

public class CategoryAppService: ICategoryAppService
{

    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;


    public CategoryAppService(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
    {
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
    }


    public async Task CreateAsync(CategoryRequest request)
    {
        Category category = new Category()
        {
            CreatedAt = DateTime.Now,
            Name = request.Name,
            Description = request.Description,
            IsActive = true,
        };
        await _categoryRepository.AddAsync(category);
        
        _unitOfWork.Commit();
    }

    public async Task<IEnumerable<AllCategoriesOutPut>> GetAllAsync()
    {
        List<Category> categories = await _categoryRepository.GetAllAsync();

        return categories.Select(c => new AllCategoriesOutPut
        {
            Name = c.Name,
            Description = c.Description,
            Id = c.Id,
        }).ToList();
    }
}
