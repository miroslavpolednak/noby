﻿using CIS.InternalServices.DataAggregatorService.Api.Configuration.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CIS.InternalServices.DataAggregatorService.Api.Configuration.Data.EntityConfiguration;

internal class DocumentSpecialDataFieldConfiguration : IEntityTypeConfiguration<DocumentSpecialDataField>
{
    public void Configure(EntityTypeBuilder<DocumentSpecialDataField> builder)
    {
        builder.HasKey(x => new { x.DocumentId, x.AcroFieldName });

        builder.Property(x => x.AcroFieldName).HasMaxLength(100).IsRequired();

        builder.Property(x => x.FieldPath).HasMaxLength(250);

        builder.Property(x => x.StringFormat).HasMaxLength(50).IsRequired(false);

        builder.HasOne(x => x.Document);

        builder.HasOne(x => x.DataService).WithMany().IsRequired();
    }
}