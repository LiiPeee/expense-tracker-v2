using System;
using ExpenseTrackerV2.Core.Domain.Entities;


namespace ExpenseTrackerV2.Infrastructure.Persistence.Dbcontext;

public class AccountConfiguration : Account
{
    public void Configure(Account account)
    {
        account.HasKey(a => a.Id);
        account.Property(a => a.Name).IsRequired().HasMaxLength(100);
        account.Property(a => a.Balance).IsRequired().HasColumnType("decimal(18,2)");
        account.Property(a => a.CreatedAt).IsRequired();
        account.HasMany(a => a.Transactions)
                .WithOne(t => t.Account)
                .HasForeignKey(t => t.AccountId);
        account.HasOne(a => a.Organization)
                .WithMany(o => o.Accounts)
                .HasForeignKey(a => a.OrganizationId);
    }

}
