using CIS.InternalServices.DataAggregator.Configuration.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CIS.InternalServices.DataAggregator.Configuration.Data.EntityConfiguration;

internal class DocumentTableConfiguration : IEntityTypeConfiguration<DocumentTable>
{
    public void Configure(EntityTypeBuilder<DocumentTable> builder)
    {
        builder.HasKey(x => x.DocumentTableId);

        builder.Property(x => x.DocumentVersion).HasMaxLength(5).IsRequired();

        builder.Property(x => x.AcroFieldPlaceholderName).HasMaxLength(100).IsRequired();

        builder.Property(x => x.ConcludingParagraph).IsRequired(false);

        builder.HasOne(x => x.Document).WithMany().IsRequired();

        builder.HasOne(x => x.DataField).WithMany().IsRequired();
    }
}