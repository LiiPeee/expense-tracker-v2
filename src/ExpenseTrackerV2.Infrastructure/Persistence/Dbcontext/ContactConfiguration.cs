using System;
using ExpenseTrackerV2.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpenseTrackerV2.Infrastructure.Persistence.Dbcontext;

public class ContactConfiguration : IEntityTypeConfiguration<Contact>
{

    public void Configure(EntityTypeBuilder<Contact> contact)
    {
        contact.HasKey(c => c.Id);
        contact.Property(c => c.Name).IsRequired().HasMaxLength(100);
        contact.Property(c => c.Email).IsRequired().HasMaxLength(200);
        contact.Property(c => c.Phone).HasMaxLength(20);
        contact.Property(c => c.CreatedAt).IsRequired();
        contact.HasOne(c => c.Address)
               .WithOne(a => a.Contact)
               .HasForeignKey<Address>(a => a.ContactId);
    }
}
