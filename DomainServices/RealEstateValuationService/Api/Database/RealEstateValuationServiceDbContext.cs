using Microsoft.EntityFrameworkCore;
using CIS.Infrastructure.Data;
using DomainServices.RealEstateValuationService.Api.Database.Entities;

namespace DomainServices.RealEstateValuationService.Api.Database;

internal sealed class RealEstateValuationServiceDbContext
    : BaseDbContext<RealEstateValuationServiceDbContext>
{
    public RealEstateValuationServiceDbContext(BaseDbContextAggregate<RealEstateValuationServiceDbContext> aggregate)
        : base(aggregate) { }

    public DbSet<RealEstateValuation> RealEstateValuations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.RegisterCisTemporalTable<RealEstateValuation>();
    }
}
