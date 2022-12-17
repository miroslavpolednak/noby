using CIS.InternalServices.DataAggregator.Configuration.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CIS.InternalServices.DataAggregator.Configuration.Data.EntityConfiguration;

internal class DocumentDynamicInputParameterConfiguration : IEntityTypeConfiguration<DocumentDynamicInputParameter>
{
    public void Configure(EntityTypeBuilder<DocumentDynamicInputParameter> builder)
    {
        builder.HasKey(x => new { x.DocumentId, x.DocumentVersion, x.InputParameterId, x.TargetDataServiceId });

        builder.Property(x => x.DocumentVersion).HasMaxLength(5);

        builder.HasOne(x => x.Document);

        builder.HasOne(x => x.InputParameter);

        builder.HasOne(x => x.TargetDataService).WithMany().OnDelete(DeleteBehavior.NoAction).HasForeignKey(x => x.TargetDataServiceId).IsRequired();

        builder.HasOne(x => x.SourceDataField).WithMany().OnDelete(DeleteBehavior.NoAction).HasForeignKey(x => x.SourceDataFieldId).IsRequired();
    }
}