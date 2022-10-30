﻿using CIS.InternalServices.DocumentDataAggregator.Configuration.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CIS.InternalServices.DocumentDataAggregator.Configuration.Data.EntityConfiguration;

internal class DocumentSpecialDataFieldConfiguration : IEntityTypeConfiguration<DocumentSpecialDataField>
{
    public void Configure(EntityTypeBuilder<DocumentSpecialDataField> builder)
    {
        builder.HasKey(x => new { x.DocumentId, x.FieldPath });

        builder.Property(x => x.FieldPath).HasMaxLength(500);

        builder.Property(x => x.TemplateFieldName).HasMaxLength(100).IsRequired();

        builder.HasOne(x => x.Document);

        builder.HasOne(x => x.DataService).WithMany().IsRequired();
    }
}