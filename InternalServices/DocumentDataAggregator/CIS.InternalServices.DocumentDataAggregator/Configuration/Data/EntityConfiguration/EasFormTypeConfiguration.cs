﻿using CIS.InternalServices.DocumentDataAggregator.Configuration.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CIS.InternalServices.DocumentDataAggregator.Configuration.Data.EntityConfiguration;

internal class EasFormTypeConfiguration : IEntityTypeConfiguration<EasFormType>
{
    public void Configure(EntityTypeBuilder<EasFormType> builder)
    {
        builder.HasKey(x => x.EasFormTypeId);

        builder.Property(x => x.EasFormTypeName).HasMaxLength(50).IsRequired();
    }
}