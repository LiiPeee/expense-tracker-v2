using ExpenseTrackerV2.Core.Domain.Entities;
using ExpenseTrackerV2.Core.Domain.Repository;
using ExpenseTrackerV2.Infrastructure.Persistence.Repository;
using System;
using System.Security.Principal;

namespace ExpenseTrackerV2.Core.Domain.UnitOfWork;

public interface IUnitOfWork
{
    RepositoryBase<T> GetRepository<T>() where T : class, IBaseEntity;

    CategoryRepository CategoryRepository { get; }
    AddressRepository AddressRepository { get; }
    ContactRepository ContactRepository { get; }
    AccountRepository AccountRepository { get; }
    TransactionRepository TransactionRepository { get; }
    OrganizationRepository OrganizationRepository { get; }


    Task BeginTransactionAsync();
    Task CommitAsync();
    Task RollbackAsync();
}
