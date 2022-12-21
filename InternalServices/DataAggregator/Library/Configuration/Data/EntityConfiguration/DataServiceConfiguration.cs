﻿using CIS.InternalServices.DataAggregator.Configuration.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CIS.InternalServices.DataAggregator.Configuration.Data.EntityConfiguration;

internal class DataServiceConfiguration : IEntityTypeConfiguration<DataService>
{
    public void Configure(EntityTypeBuilder<DataService> builder)
    {
        builder.HasKey(x => x.DataServiceId);

        builder.Property(x => x.DataServiceId).ValueGeneratedNever();

        builder.Property(x => x.DataServiceName).HasMaxLength(50).IsRequired();
    }
}