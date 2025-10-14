using System;
using ExpenseTrackerV2.Core.Domain.Entities;
using ExpenseTrackerV2.Core.Domain.Repository;

namespace ExpenseTrackerV2.Infrastructure.Persistence.Repository;

public class AddressRepository : RepositoryBase<Address>, IAddressRepository
{
    public AddressRepository(DapperContext context) : base(context)
    {
    }
}
