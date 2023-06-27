using CIS.Infrastructure.Data;
using DomainServices.RealEstateValuationService.Api.Database.Entities;

namespace DomainServices.RealEstateValuationService.Api.Database;

internal sealed class RealEstateValuationServiceDbContext
    : BaseDbContext<RealEstateValuationServiceDbContext>
{
    public RealEstateValuationServiceDbContext(BaseDbContextAggregate<RealEstateValuationServiceDbContext> aggregate)
        : base(aggregate) { }

    public DbSet<RealEstateValuation> RealEstateValuations { get; set; }
    public DbSet<RealEstateValuationDetail> RealEstateValuationDetails { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.RegisterCisTemporalTable<RealEstateValuation>();
        modelBuilder.RegisterCisTemporalTable<RealEstateValuationDetail>();
    }
}
