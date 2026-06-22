using BudgetTracker.Application.Service;
using BudgetTracker.Core.Domain.Entities;
using BudgetTracker.Core.Domain.Models.Request;
using BudgetTracker.Core.Domain.UnitOfWork;
using BudgetTracker.Core.Infrastructure.Repository;
using Moq;

namespace Test;

public class SubCategoryServiceTest
{
    private Mock<ISubCategoryRepository> _subCategoryRepo;
    private Mock<IUnitOfWork>            _unitOfWork;
    private SubCategoryAppService        _service;

    [SetUp]
    public void Setup()
    {
        _subCategoryRepo = new Mock<ISubCategoryRepository>();
        _unitOfWork      = new Mock<IUnitOfWork>();
        _service         = new SubCategoryAppService(_subCategoryRepo.Object, _unitOfWork.Object);
    }

    private static CreateSubCategoryRequest BuildRequest() => new()
    {
        Name        = "Aluguel",
        Description = "Despesa fixa",
        IsActive    = true,
        CategoryId  = 1,
    };

    [Test]
    public async Task CreateAsync_ValidRequest_SavesSubCategoryAndCommits()
    {
        _subCategoryRepo.Setup(r => r.AddAsync(It.IsAny<SubCategory>()))
            .ReturnsAsync(new SubCategory { Id = 1, Name = "Aluguel" });

        await _service.CreateAsync(accountId: 42, BuildRequest());

        _subCategoryRepo.Verify(r => r.AddAsync(It.Is<SubCategory>(s =>
            s.Name == "Aluguel" &&
            s.AccountId == 42 &&
            s.IsActive == true)), Times.Once);

        _unitOfWork.Verify(u => u.BeginTransaction(), Times.Once);
        _unitOfWork.Verify(u => u.Commit(), Times.Once);
    }

    [Test]
    public async Task CreateAsync_RepositoryThrows_RollsBackAndRethrows()
    {
        _subCategoryRepo.Setup(r => r.AddAsync(It.IsAny<SubCategory>())).ThrowsAsync(new Exception("db error"));

        Assert.ThrowsAsync<Exception>(() => _service.CreateAsync(accountId: 42, BuildRequest()));

        _unitOfWork.Verify(u => u.Rollback(), Times.Once);
        _unitOfWork.Verify(u => u.Commit(), Times.Never);
    }

    [Test]
    public async Task GetAllAsync_ReturnsAllSubCategories()
    {
        var data = new List<SubCategory>
        {
            new() { Id = 1, Name = "Aluguel" },
            new() { Id = 2, Name = "Luz" },
        };
        _subCategoryRepo.Setup(r => r.GetAllAsync(42)).ReturnsAsync(data);

        var result = (await _service.GetAllAsync(42)).ToList();

        Assert.That(result, Has.Count.EqualTo(2));
        Assert.That(result[0].Name, Is.EqualTo("Aluguel"));
        _subCategoryRepo.Verify(r => r.GetAllAsync(42), Times.Once);
    }
}
