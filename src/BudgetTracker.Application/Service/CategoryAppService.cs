using BudgetTracker.Application.Dtos.Request;
using BudgetTracker.Core.Domain.Entities;
using BudgetTracker.Core.Domain.Models.Output;
using BudgetTracker.Core.Domain.Repository;
using BudgetTracker.Core.Domain.Service;
using BudgetTracker.Core.Domain.UnitOfWork;

namespace BudgetTracker.Application.Service;

public class CategoryAppService : ICategoryAppService
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


