using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using ExpenseTrackerV2.Core.Domain.Repository;
using ExpenseTrackerV2.Core.Infrastructure.Repository;
using ExpenseTrackerV2.Core.Infrastructure.UnitOfWork;
using ExpenseTrackerV2.Infrastructure.UnitOfWork;
using Microsoft.Extensions.Configuration;

namespace ExpenseTrackerV2.Infrastructure.Persistence.Repository;

public class DbSession : IDbsession
{
    private IUnitOfWorkFactory _unitOfWorkFactory;

    private UnitOfWork _unitOfWork;

    private ITransactionsRepository _transactionsRepository;

    private IOrganizationRepository _organizationRepository;

    private IAddressRepository _addressRepository;

    private IAccountRepository _accountRepository;

    private ICategoryRepository _categoryRepository;

    private ISubCategoryRepository _subCategoryRepository;



    public DbSession(IUnitOfWorkFactory unitOfWorkFactory)
    {
        this._unitOfWorkFactory = unitOfWorkFactory;
    }

    public TransactionsRepository Transactions =>
        _transactionsRepository ?? (_transactionsRepository = new TransactionsRepository(UnitOfWork));

    proctected UnitOfWork UnitOfWork =>
        _unitOfWork ?? (_unitOfWork = _unitOfWorkFactory.Create());

   

    public void CommitTransaction()
    {
        try
        {
            UnitOfWork.CommitTransaction();
        }
        finally
        {
            Reset();
        }
    }

    public void RollbackTransaction()
    {
        try
        {
            UnitOfWork?.Rollback();
        }
        finally
        {
            Reset();
        }
    }

    private void Reset()
    {
        _unitOfWork = null;
        _transactionsRepository = null;
    }
}
