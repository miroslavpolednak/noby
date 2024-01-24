﻿using CIS.Infrastructure.Data;
using DomainServices.RealEstateValuationService.Api.Database.Entities;

namespace DomainServices.RealEstateValuationService.Api.Database;

internal sealed class RealEstateValuationServiceDbContext
    : BaseDbContext<RealEstateValuationServiceDbContext>
{
    public RealEstateValuationServiceDbContext(BaseDbContextAggregate<RealEstateValuationServiceDbContext> aggregate)
        : base(aggregate) { }

    public DbSet<RealEstateValuation> RealEstateValuations { get; set; }
    public DbSet<DeedOfOwnershipDocument> DeedOfOwnershipDocuments { get; set; }
    public DbSet<RealEstateValuationAttachment> Attachments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.RegisterCisTemporalTable<RealEstateValuation>();
        modelBuilder.RegisterCisTemporalTable<DeedOfOwnershipDocument>();
        modelBuilder.RegisterCisTemporalTable<RealEstateValuationAttachment>();

        modelBuilder.Entity<RealEstateValuation>().OwnsMany(p => p.Prices, b => b.ToJson());
    }
}
