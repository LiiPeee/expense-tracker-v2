using System;
using ExpenseTrackerV2.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpenseTrackerV2.Infrastructure.Persistence.Dbcontext;

public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> transaction)
    {
        transaction.HasKey(t => t.Id);
        transaction.Property(t => t.Amount).IsRequired().HasColumnType("decimal(18,2)");
        transaction.Property(t => t.Description).HasMaxLength(500);
        transaction.HasOne(t => t.Account);
        transaction.HasOne(t => t.Category);
        transaction.Property(t => t.CreatedAt).IsRequired();
        transaction.Property(t => t.Paid).IsRequired();
        transaction.Property(t => t.NumberOfInstallment).IsRequired();
        transaction.Property(t => t.DateOfInstallment).IsRequired();
        transaction.Property(t => t.Recurrence).IsRequired();
        transaction.HasOne(t => t.Contact);
        transaction.Property(t => t.Category).IsRequired();
        transaction.Property(t => t.AccountId).IsRequired();
    }

}
