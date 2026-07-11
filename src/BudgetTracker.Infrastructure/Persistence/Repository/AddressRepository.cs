using BudgetTracker.Core.Domain.Entities;
using BudgetTracker.Core.Domain.Repository;
using System.Data;

namespace BudgetTracker.Infrastructure.Persistence.Repository;

public class AddressRepository : AccountScopedRepositoryBase<Address>, IAddressRepository
{
    public AddressRepository(DbSession connection) : base(connection)
    {
    }
}


