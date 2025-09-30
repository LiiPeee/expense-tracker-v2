
using ExpenseTrackerV2.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace ExpenseTrackerV2.Infrastructure.Persistence.Dbcontext;

public class OrganizationConfiguration : IEntityTypeConfiguration<Organization>
{
    public void Configure(EntityTypeBuilder<Organization> organization)
    {
        organization.HasKey(o => o.Id);
        organization.Property(o => o.Name).IsRequired().HasMaxLength(100);
        organization.Property(o => o.CreatedAt).IsRequired();
        organization.HasMany(o => o.Accounts)
                .WithOne(a => a.Organization)
                .HasForeignKey(a => a.OrganizationId);
    }

}
