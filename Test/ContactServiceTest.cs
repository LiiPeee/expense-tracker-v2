using BudgetTracker.Application.Dtos.Request;
using BudgetTracker.Application.Service;
using BudgetTracker.Core.Domain.Entities;
using BudgetTracker.Core.Domain.Enum;
using BudgetTracker.Core.Domain.Models.Request.Transaction;
using BudgetTracker.Core.Domain.Repository;
using BudgetTracker.Core.Domain.UnitOfWork;
using BudgetTracker.Core.Infrastructure.Repository;
using Moq;

namespace Test;

public class ContactServiceTest
{
    private Mock<IContactRepository>  _contactRepo;
    private Mock<IAddressRepository>  _addressRepo;
    private Mock<IUnitOfWork>          _unitOfWork;
    private ContactAppService          _service;

    [SetUp]
    public void Setup()
    {
        _contactRepo = new Mock<IContactRepository>();
        _addressRepo = new Mock<IAddressRepository>();
        _unitOfWork  = new Mock<IUnitOfWork>();
        _service     = new ContactAppService(_contactRepo.Object, _addressRepo.Object, _unitOfWork.Object);
    }

    private static CreateContactRequest BuildCreateRequest() => new()
    {
        Name        = "Alice",
        Email       = "alice@test.com",
        Phone       = "999",
        Document    = "123",
        TypeContact = TypeContact.PERSONAL,
        Street      = "Main St",
        City        = "SP",
        State       = "SP",
        ZipCode     = "01000-000",
        Country     = "Brazil",
        IsPrimary   = true,
    };

    private static ContactRequest BuildRequest(string contactId = "") => new()
    {
        ContactId   = contactId,
        Name        = "Alice",
        Email       = "alice@test.com",
        Phone       = "999",
        Document    = "123",
        TypeContact = TypeContact.PERSONAL,
        Street      = "Main St",
        City        = "SP",
        State       = "SP",
        ZipCode     = "01000-000",
        Country     = "Brazil",
        IsPrimary   = true,
    };

    // ── CreateAsync ──────────────────────────────────────────────────────────

    [Test]
    public async Task CreateAsync_ValidRequest_SavesContactAndCommits()
    {
        var savedContact = new Contact { Id = 10, Name = "Alice", AccountId = 1 };
        var savedAddress = new Address { Id = 1, City = "SP", ContactId = 10 };

        _contactRepo.Setup(r => r.AddAsync(It.IsAny<Contact>())).ReturnsAsync(savedContact);
        _addressRepo.Setup(r => r.AddAsync(It.IsAny<Address>())).ReturnsAsync(savedAddress);

        var result = await _service.CreateAsync(1, BuildCreateRequest());

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Id, Is.EqualTo(10));
        _unitOfWork.Verify(u => u.BeginTransaction(), Times.Once);
        _unitOfWork.Verify(u => u.Commit(), Times.Once);
        // Verifica que o Address foi salvo com o ContactId correto
        _addressRepo.Verify(r => r.AddAsync(It.Is<Address>(a => a.ContactId == 10)), Times.Once);
    }

    [Test]
    public async Task CreateAsync_ContactRepositoryThrows_RollsBackAndRethrows()
    {
        _contactRepo.Setup(r => r.AddAsync(It.IsAny<Contact>())).ThrowsAsync(new Exception("db error"));

        Assert.ThrowsAsync<Exception>(() => _service.CreateAsync(1, BuildCreateRequest()));

        _unitOfWork.Verify(u => u.Rollback(), Times.Once);
        _unitOfWork.Verify(u => u.Commit(), Times.Never);
    }

    [Test]
    public async Task CreateAsync_AddressRepositoryThrows_RollsBackAndRethrows()
    {
        _contactRepo.Setup(r => r.AddAsync(It.IsAny<Contact>())).ReturnsAsync(new Contact { Id = 5 });
        _addressRepo.Setup(r => r.AddAsync(It.IsAny<Address>())).ThrowsAsync(new Exception("db error"));

        Assert.ThrowsAsync<Exception>(() => _service.CreateAsync(1, BuildCreateRequest()));

        _unitOfWork.Verify(u => u.Rollback(), Times.Once);
        _unitOfWork.Verify(u => u.Commit(), Times.Never);
    }

    [Test]
    public async Task CreateAsync_SetsAccountIdOnContact()
    {
        Contact? captured = null;
        _contactRepo.Setup(r => r.AddAsync(It.IsAny<Contact>()))
            .Callback<Contact>(c => captured = c)
            .ReturnsAsync(new Contact { Id = 5 });
        _addressRepo.Setup(r => r.AddAsync(It.IsAny<Address>())).ReturnsAsync(new Address { Id = 1, ContactId = 5 });

        await _service.CreateAsync(42, BuildCreateRequest());

        Assert.That(captured!.AccountId, Is.EqualTo(42));
    }

    // ── GetAllAsync ──────────────────────────────────────────────────────────

    [Test]
    public async Task GetAllsync_ReturnsContactsForAccount()
    {
        var contacts = new List<Contact?> { new() { Id = 1, AccountId = 1 }, new() { Id = 2, AccountId = 1 } };
        _contactRepo.Setup(r => r.GetByIdAccount(1)).ReturnsAsync(contacts);

        var result = await _service.GetAllsync(1);

        Assert.That(result, Has.Count.EqualTo(2));
    }

    // ── EditContactAsync ─────────────────────────────────────────────────────

    [Test]
    public async Task EditContactAsync_ValidContact_UpdatesAndCommits()
    {
        var existing = new Contact { Id = 5, AccountId = 1, Name = "Old" };
        _contactRepo.Setup(r => r.GetByIdAsync(5)).ReturnsAsync(existing);
        _contactRepo.Setup(r => r.UpdateAsync(It.IsAny<Contact>())).ReturnsAsync(true);

        await _service.EditContactAsync(1, BuildRequest(contactId: "5"));

        _contactRepo.Verify(r => r.UpdateAsync(It.Is<Contact>(c => c.Name == "Alice")), Times.Once);
        _unitOfWork.Verify(u => u.BeginTransaction(), Times.Once);
        _unitOfWork.Verify(u => u.Commit(), Times.Once);
    }

    [Test]
    public async Task EditContactAsync_RepositoryThrows_RollsBackAndRethrows()
    {
        var existing = new Contact { Id = 5, AccountId = 1 };
        _contactRepo.Setup(r => r.GetByIdAsync(5)).ReturnsAsync(existing);
        _contactRepo.Setup(r => r.UpdateAsync(It.IsAny<Contact>())).ThrowsAsync(new Exception("db error"));

        Assert.ThrowsAsync<Exception>(() => _service.EditContactAsync(1, BuildRequest(contactId: "5")));

        _unitOfWork.Verify(u => u.Rollback(), Times.Once);
        _unitOfWork.Verify(u => u.Commit(), Times.Never);
    }

    [Test]
    public async Task EditContactAsync_ContactBelongsToDifferentAccount_ThrowsUnauthorized()
    {
        var existing = new Contact { Id = 5, AccountId = 99 };
        _contactRepo.Setup(r => r.GetByIdAsync(5)).ReturnsAsync(existing);

        Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => _service.EditContactAsync(1, BuildRequest(contactId: "5")));
    }

    [Test]
    public async Task EditContactAsync_InvalidContactId_ThrowsArgumentException()
    {
        Assert.ThrowsAsync<ArgumentException>(
            () => _service.EditContactAsync(1, BuildRequest(contactId: "not-a-number")));
    }

    // ── DeleteContactAsync ───────────────────────────────────────────────────

    [Test]
    public async Task DeleteContactAsync_ValidContact_DeletesAndCommits()
    {
        var existing = new Contact { Id = 3, AccountId = 1 };
        _contactRepo.Setup(r => r.GetByIdAsync(3)).ReturnsAsync(existing);
        _contactRepo.Setup(r => r.DeleteAsync(3)).ReturnsAsync(true);

        await _service.DeleteContactAsync(1, "3");

        _contactRepo.Verify(r => r.DeleteAsync(3), Times.Once);
        _unitOfWork.Verify(u => u.BeginTransaction(), Times.Once);
        _unitOfWork.Verify(u => u.Commit(), Times.Once);
    }

    [Test]
    public async Task DeleteContactAsync_RepositoryThrows_RollsBackAndRethrows()
    {
        var existing = new Contact { Id = 3, AccountId = 1 };
        _contactRepo.Setup(r => r.GetByIdAsync(3)).ReturnsAsync(existing);
        _contactRepo.Setup(r => r.DeleteAsync(3)).ThrowsAsync(new Exception("db error"));

        Assert.ThrowsAsync<Exception>(() => _service.DeleteContactAsync(1, "3"));

        _unitOfWork.Verify(u => u.Rollback(), Times.Once);
        _unitOfWork.Verify(u => u.Commit(), Times.Never);
    }

    [Test]
    public async Task DeleteContactAsync_ContactBelongsToDifferentAccount_ThrowsUnauthorized()
    {
        var existing = new Contact { Id = 3, AccountId = 77 };
        _contactRepo.Setup(r => r.GetByIdAsync(3)).ReturnsAsync(existing);

        Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => _service.DeleteContactAsync(1, "3"));
    }
}
