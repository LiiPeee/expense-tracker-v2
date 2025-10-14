using System;
using ExpenseTrackerV2.Application.Dtos.Request;
using ExpenseTrackerV2.Core.Domain.Entities;
using ExpenseTrackerV2.Core.Domain.Repository;

namespace ExpenseTrackerV2.Application.Service;

public class CategoryAppService(ICategoryRepository categoryRepository)
{

    private readonly ICategoryRepository _categoryRepository = categoryRepository;


    public async Task<Category> CreateAsync(CategoryRequest request)
    {


    }

}
