using System;
using ExpenseTrackerV2.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpenseTrackerV2.Infrastructure.Persistence.Dbcontext;

public class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> account)
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
