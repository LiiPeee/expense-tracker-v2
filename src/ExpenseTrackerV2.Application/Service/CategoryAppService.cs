using System;
using ExpenseTrackerV2.Application.Dtos.Request;
using ExpenseTrackerV2.Core.Domain.Entities;
using ExpenseTrackerV2.Core.Domain.Repository;
using ExpenseTrackerV2.Core.Domain.Service;

namespace ExpenseTrackerV2.Application.Service;

public class CategoryAppService: ICategoryAppService
{

    private readonly ICategoryRepository _categoryRepository;

    public CategoryAppService(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
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
    }

}
