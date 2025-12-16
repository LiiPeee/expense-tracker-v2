using ExpenseTrackerV2.Core.Domain.Repository;
using ExpenseTrackerV2.Core.Infrastructure.Repository;

namespace ExpenseTrackerV2.Core.Infrastructure.UnitOfWork
{
    public interface IDbsession
    {
        ITransactionsRepository TransactionsRepository { get; set; }
        IAccountRepository AccountRepository { get; set; }
        IAddressRepository AddressRepository { get; set; }
        IOrganizationRepository OrganizationRepository { get; set; }
        ICategoryRepository CategoryRepository { get; set; }
        ISubCategoryRepository SubCategoryRepository { get; set; }


        void Commit();
        void Rollback();
    }
}
