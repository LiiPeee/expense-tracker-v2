using System;
using ExpenseTrackerV2.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpenseTrackerV2.Infrastructure.Persistence.Dbcontext;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> category)
    {
        category.HasKey(c => c.Id);
        category.Property(c => c.Name).IsRequired().HasMaxLength(100);
        category.Property(c => c.Description).HasMaxLength(500);
        category.Property(c => c.CreatedAt).IsRequired();
        category.Property(c => c.IsActive).IsRequired();
        category.HasMany(c => c.Transactions)
                .WithOne(t => t.Category)
                .HasForeignKey(t => t.CategoryId);
    }

}
