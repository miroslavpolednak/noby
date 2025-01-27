﻿using Microsoft.EntityFrameworkCore;
using CIS.Infrastructure.Data;

namespace DomainServices.SalesArrangementService.Api.Database;

internal sealed class SalesArrangementServiceDbContext(BaseDbContextAggregate<SalesArrangementServiceDbContext> aggregate)
        : BaseDbContext<SalesArrangementServiceDbContext>(aggregate)
{
    public DbSet<Entities.FormValidationTransformation> FormValidationTransformations => Set<Entities.FormValidationTransformation>();
    public DbSet<Entities.SalesArrangement> SalesArrangements => Set<Entities.SalesArrangement>();
    public DbSet<Entities.FlowSwitch> FlowSwitches => Set<Entities.FlowSwitch>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.RegisterCisTemporalTable<Entities.SalesArrangement>();
        modelBuilder.RegisterCisTemporalTable<Entities.FlowSwitch>();
    }
}
