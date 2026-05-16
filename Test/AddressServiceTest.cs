using BudgetTracker.Application.Dtos.Request;
using BudgetTracker.Application.Service;
using BudgetTracker.Core.Domain.Entities;
using BudgetTracker.Core.Domain.Repository;
using BudgetTracker.Core.Domain.UnitOfWork;
using BudgetTracker.Core.Infrastructure.Repository;
using Moq;

namespace Test;

public class AddressServiceTest
{
    private Mock<IAddressRepository> _addressRepo;
    private Mock<IContactRepository> _contactRepo;
    private Mock<IUnitOfWork>        _unitOfWork;
    private AddressAppService        _service;

    [SetUp]
    public void Setup()
    {
        _addressRepo = new Mock<IAddressRepository>();
        _contactRepo = new Mock<IContactRepository>();
        _unitOfWork  = new Mock<IUnitOfWork>();
        _service     = new AddressAppService(_contactRepo.Object, _addressRepo.Object, _unitOfWork.Object);
    }

    private static AddressRequest BuildRequest() => new()
    {
        Street      = "Rua A",
        City        = "SP",
        State       = "SP",
        ZipCode     = "01000-000",
        Country     = "Brazil",
        ContactName = "Test",
        IsPrimary   = true,
    };

    [Test]
    public async Task CreateAsync_ValidRequest_SavesAddressAndCommits()
    {
        _addressRepo.Setup(r => r.AddAsync(It.IsAny<Address>())).ReturnsAsync(new Address { Id = 1, City = "SP" });

        await _service.CreateAsync(BuildRequest());

        _addressRepo.Verify(r => r.AddAsync(It.Is<Address>(a =>
            a.City == "SP" &&
            a.IsPrimary == true)), Times.Once);

        _unitOfWork.Verify(u => u.BeginTransaction(), Times.Once);
        _unitOfWork.Verify(u => u.Commit(), Times.Once);
    }

    [Test]
    public async Task CreateAsync_RepositoryThrows_RollsBackAndRethrows()
    {
        _addressRepo.Setup(r => r.AddAsync(It.IsAny<Address>())).ThrowsAsync(new Exception("db error"));

        Assert.ThrowsAsync<Exception>(() => _service.CreateAsync(BuildRequest()));

        _unitOfWork.Verify(u => u.Rollback(), Times.Once);
        _unitOfWork.Verify(u => u.Commit(), Times.Never);
    }
}
