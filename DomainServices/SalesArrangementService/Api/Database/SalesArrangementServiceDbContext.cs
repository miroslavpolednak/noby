using Microsoft.EntityFrameworkCore;
using CIS.Infrastructure.Data;

namespace DomainServices.SalesArrangementService.Api.Database;

internal sealed class SalesArrangementServiceDbContext 
    : BaseDbContext<SalesArrangementServiceDbContext>
{
#pragma warning disable CS8618
    public SalesArrangementServiceDbContext(BaseDbContextAggregate<SalesArrangementServiceDbContext> aggregate)
#pragma warning restore CS8618
        : base(aggregate) { }

    public DbSet<Entities.FormValidationTransformation> FormValidationTransformations { get; set; }
    public DbSet<Entities.SalesArrangement> SalesArrangements { get; set; }
    public DbSet<Entities.SalesArrangementParameters> SalesArrangementsParameters { get; set; }
    public DbSet<Entities.FlowSwitch> FlowSwitches { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.RegisterCisTemporalTable<Entities.SalesArrangement>();
        modelBuilder.RegisterCisTemporalTable<Entities.SalesArrangementParameters>();
        modelBuilder.RegisterCisTemporalTable<Entities.FlowSwitch>();
    }
}
