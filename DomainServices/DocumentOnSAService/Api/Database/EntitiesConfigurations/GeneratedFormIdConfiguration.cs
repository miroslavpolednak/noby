using DomainServices.DocumentOnSAService.Api.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DomainServices.DocumentOnSAService.Api.Database.EntitiesConfigurations;

public class GeneratedFormIdConfiguration : IEntityTypeConfiguration<GeneratedFormId>
{
    public void Configure(EntityTypeBuilder<GeneratedFormId> builder)
    {
        builder.HasKey(e => e.Id);

        builder.HasIndex(e => e.HouseholdId);

        builder.Property(e => e.TargetSystem)
            .HasColumnType("nvarchar(2)")
            .HasDefaultValue("N");
    }
}
