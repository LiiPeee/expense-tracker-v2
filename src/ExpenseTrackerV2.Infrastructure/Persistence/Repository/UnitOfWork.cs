

namespace ExpenseTrackerV2.Infrastructure.Persistence.Repository
{
    using ExpenseTrackerV2.Core.Domain.Entities;
    using ExpenseTrackerV2.Core.Domain.UnitOfWork;
    using Microsoft.EntityFrameworkCore;
    using System.Threading.Tasks;

    public class UnitOfWork : IUnitOfWork
    {
        private CategoryRepository? _categoryRepository;
        private AddressRepository? _addressRepository;
        private ContactRepository? _contactRepository;
        private AccountRepository? _accountRepository;
        private TransactionsRepository? _transactionRepository;
        private OrganizationRepository? _organizationRepository;
        private readonly DapperContext _context;
        
        public UnitOfWork(DapperContext context)
        {
            _context = context;
        }
        public CategoryRepository CategoryRepository => _categoryRepository ??= new CategoryRepository(_context);
        public AddressRepository AddressRepository => _addressRepository ??= new AddressRepository(_context);
        public ContactRepository ContactRepository => _contactRepository ??= new ContactRepository(_context);
        public AccountRepository AccountRepository => _accountRepository ??= new AccountRepository(_context);
        public TransactionsRepository TransactionRepository => _transactionRepository ??= new TransactionsRepository(_context);
        public OrganizationRepository OrganizationRepository => _organizationRepository ??= new OrganizationRepository(_context);

        public RepositoryBase<T> GetRepository<T>() where T : class, IBaseEntity
        {
            return new RepositoryBase<T>(_context);
        }
        public Task BeginTransactionAsync() =>
             _context.BeginTransactionAsync();
        

        public Task CommitAsync()
        {
            _context.CommitTransaction();
            return Task.CompletedTask;
        }

   

        public Task RollbackAsync()
        {
            _context.RollbackTransaction();
            return Task.CompletedTask;    
        }
    }
}
