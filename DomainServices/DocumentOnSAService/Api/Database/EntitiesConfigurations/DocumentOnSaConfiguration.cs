using DomainServices.DocumentOnSAService.Api.Database.Entities;
using DomainServices.DocumentOnSAService.Api.Database.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DomainServices.DocumentOnSAService.Api.Database.EntitiesConfigurations;

public class DocumentOnSaConfiguration : IEntityTypeConfiguration<DocumentOnSa>
{
    public void Configure(EntityTypeBuilder<DocumentOnSa> builder)
    {
        builder.HasKey(e => e.DocumentOnSAId);

        builder.HasIndex(e => e.SalesArrangementId);

        builder.Property(e => e.FormId)
                    .HasColumnType("nvarchar(15)");

        builder.Property(e => e.EArchivId)
           .HasColumnType("nvarchar(50)");

        builder.Property(e => e.DmsxId)
           .HasColumnType("nvarchar(50)");

        builder.Property(e => e.CreatedUserName)
           .HasColumnType("nvarchar(50)");

        builder.Property(e => e.SignatureMethodCode)
         .HasColumnType("nvarchar(15)");

        builder.Property(e => e.IsValid)
           .HasDefaultValue(true);

        builder.Property(e => e.IsSigned)
           .HasDefaultValue(false);

        builder.Property(e => e.IsArchived)
           .HasDefaultValue(false);

        builder.Property(e => e.ExternalId)
          .HasColumnType("nvarchar(50)");

        builder.Property(e => e.Source)
            .HasDefaultValue(Source.Noby);

        if (DocumentOnSAServiceDbContext.IsSqlite)
            builder.Property(e => e.Data).HasColumnType("text");
        else
            builder.Property(e => e.Data)
                       .HasColumnType("nvarchar(MAX)");
    }
}
