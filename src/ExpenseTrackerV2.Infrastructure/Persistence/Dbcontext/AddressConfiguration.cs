using System;
using ExpenseTrackerV2.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpenseTrackerV2.Infrastructure.Persistence.Dbcontext;

public class AddressConfiguration : IEntityTypeConfiguration<Address>
{
    public void Configure(EntityTypeBuilder<Address> address)
    {
        address.HasKey(a => a.Id);
        address.Property(a => a.Street).IsRequired().HasMaxLength(200);
        address.Property(a => a.City).IsRequired().HasMaxLength(100);
        address.Property(a => a.State).IsRequired().HasMaxLength(100);
        address.Property(a => a.ZipCode).IsRequired().HasMaxLength(20);
        address.Property(a => a.Country).IsRequired().HasMaxLength(100);
        address.Property(a => a.CreatedAt).IsRequired();
    }
}
