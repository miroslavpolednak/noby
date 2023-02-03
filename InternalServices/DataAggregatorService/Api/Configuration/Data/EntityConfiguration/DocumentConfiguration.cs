﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CIS.InternalServices.DataAggregatorService.Api.Configuration.Data.EntityConfiguration;

internal class DocumentConfiguration : IEntityTypeConfiguration<Entities.Document>
{
    public void Configure(EntityTypeBuilder<Entities.Document> builder)
    {
        builder.HasKey(x => x.DocumentId);

        builder.Property(x => x.DocumentId).ValueGeneratedNever();

        builder.Property(x => x.DocumentName).HasMaxLength(50).IsRequired();
    }
}