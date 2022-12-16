using CIS.InternalServices.DataAggregator.Configuration.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CIS.InternalServices.DataAggregator.Configuration.Data.EntityConfiguration;

internal class EasFormTypeConfiguration : IEntityTypeConfiguration<EasFormType>
{
    public void Configure(EntityTypeBuilder<EasFormType> builder)
    {
        builder.HasKey(x => x.EasFormTypeId);

        builder.Property(x => x.EasFormTypeId).ValueGeneratedNever();

        builder.Property(x => x.EasFormTypeName).HasMaxLength(50).IsRequired();
    }
}