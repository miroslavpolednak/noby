using DomainServices.DocumentOnSAService.Api.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DomainServices.DocumentOnSAService.Api.Database.EntitiesConfigurations;

public class EArchivIdsLinkedConfiguration : IEntityTypeConfiguration<EArchivIdsLinked>
{
    public void Configure(EntityTypeBuilder<EArchivIdsLinked> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.EArchivId)
         .HasColumnType("nvarchar(50)");
    }
}
