using ExpenseTrackerV2.Core.Domain.Entities;
using ExpenseTrackerV2.Core.Domain.Repository;
using System.Data;

namespace ExpenseTrackerV2.Infrastructure.Persistence.Repository;

public class AddressRepository : RepositoryBase<Address>, IAddressRepository
{
    public AddressRepository(DbSession connection) : base(connection)
    {
    }
}
