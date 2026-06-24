using BudgetTracker.Application.Service;
using BudgetTracker.Core.Domain.Dtos.Output;
using BudgetTracker.Core.Domain.Entities;
using BudgetTracker.Core.Domain.Repository;
using BudgetTracker.Core.Domain.UnitOfWork;
using BudgetTracker.Core.Infrastructure.Repository;
using Moq;

namespace Test.Application;

/// <summary>
/// An empty month is a valid result, not an error: type filters must return an empty
/// page so the dashboard renders an empty chart instead of a failure state.
/// </summary>
[TestFixture]
public class TransactionsAppServiceQueryTests
{
    private const long AccountId = 1;

    private TransactionsAppService BuildService(IPagedResult<Transactions> pagedResult)
    {
        var transactionRepository = new Mock<ITransactionsRepository>();
        transactionRepository
            .Setup(r => r.FilterTransactionsByTypeAsync(AccountId, It.IsAny<string>(), It.IsAny<long>(), It.IsAny<long>(), It.IsAny<int>()))
            .ReturnsAsync(pagedResult);
        transactionRepository
            .Setup(r => r.FilterByMonthAndYearAsync(AccountId, It.IsAny<long>(), It.IsAny<long>(), It.IsAny<int>()))
            .ReturnsAsync(pagedResult);

        return new TransactionsAppService(
            transactionRepository.Object,
            new Mock<ICategoryRepository>().Object,
            new Mock<IContactRepository>().Object,
            new Mock<ISubCategoryRepository>().Object,
            new Mock<IAccountRepository>().Object,
            new Mock<IUnitOfWork>().Object);
    }

    [Test]
    public async Task Dado_MesSemTransacoes_Quando_FiltrarPorTipo_Entao_RetornaPaginaVazia()
    {
        var emptyPage = new IPagedResult<Transactions>
        {
            PageNumber = 1,
            PageSize = 10,
            TotalRecords = 0,
            Items = new List<Transactions>(),
        };
        var service = BuildService(emptyPage);

        var result = await service.FilterTransactionByTypeAsync(AccountId, TypeTransaction.INCOME, 6, 2026);

        Assert.Multiple(() =>
        {
            Assert.That(result.Items, Is.Empty);
            Assert.That(result.TotalRecords, Is.EqualTo(0));
            Assert.That(result.PageSize, Is.EqualTo(10));
        });
    }

    [Test]
    public async Task Dado_MesSemTransacoes_Quando_ListarPorMesEAno_Entao_RetornaPaginaVazia()
    {
        var emptyPage = new IPagedResult<Transactions>
        {
            PageNumber = 1,
            PageSize = 10,
            TotalRecords = 0,
            Items = new List<Transactions>(),
        };
        var service = BuildService(emptyPage);

        var result = await service.FilterByMonthAndYearAsync(AccountId, 6, 2026);

        Assert.That(result.Items, Is.Empty);
    }
}
