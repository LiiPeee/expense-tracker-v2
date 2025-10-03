using ExpenseTrackerV2.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTrackerV2.Infrastructure.Persistence.Dbcontext;

public class ExpenseTrackerDbContext : DbContext
{
    public readonly string? _connectionString;
    public DbSet<Organization> Organization => Set<Organization>();
    public DbSet<Account> Account => Set<Account>();
    public DbSet<Transaction> Transaction => Set<Transaction>();
    public DbSet<Category> Category => Set<Category>();
    public DbSet<Address> Address => Set<Address>();
    public DbSet<Contact> Contact => Set<Contact>();

    public ExpenseTrackerDbContext(string connectionString)
    {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
    }

    public ExpenseTrackerDbContext(DbContextOptions<ExpenseTrackerDbContext> options) : base(options)
    {
    }

}
