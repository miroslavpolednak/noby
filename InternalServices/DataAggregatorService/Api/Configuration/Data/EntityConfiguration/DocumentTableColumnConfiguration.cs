using CIS.InternalServices.DataAggregatorService.Api.Configuration.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CIS.InternalServices.DataAggregatorService.Api.Configuration.Data.EntityConfiguration;

internal class DocumentTableColumnConfiguration : IEntityTypeConfiguration<DocumentTableColumn>
{
    public void Configure(EntityTypeBuilder<DocumentTableColumn> builder)
    {
        builder.HasKey(x => new { x.DocumentTableId, x.FieldPath });

        builder.Property(x => x.FieldPath).HasMaxLength(500).IsRequired();

        builder.Property(x => x.Order).IsRequired();

        builder.Property(x => x.Header).HasMaxLength(100).IsRequired();

        builder.Property(x => x.WidthPercentage).IsRequired();

        builder.Property(x => x.StringFormat).HasMaxLength(500).IsRequired(false);
    }
}