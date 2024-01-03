using Microsoft.EntityFrameworkCore;
using CIS.Infrastructure.Data;

namespace DomainServices.SalesArrangementService.Api.Database;

internal sealed class SalesArrangementServiceDbContext(BaseDbContextAggregate<SalesArrangementServiceDbContext> aggregate)
        : BaseDbContext<SalesArrangementServiceDbContext>(aggregate)
{
    public DbSet<Entities.FormValidationTransformation> FormValidationTransformations => Set<Entities.FormValidationTransformation>();
    public DbSet<Entities.SalesArrangement> SalesArrangements => Set<Entities.SalesArrangement>();
    public DbSet<Entities.SalesArrangementParameters> SalesArrangementsParameters => Set<Entities.SalesArrangementParameters>();
    public DbSet<Entities.FlowSwitch> FlowSwitches => Set<Entities.FlowSwitch>();
    public DbSet<Queries.CaseSaParametersQuery> CaseSaParametersQuery => Set<Queries.CaseSaParametersQuery>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.RegisterCisTemporalTable<Entities.SalesArrangement>();
        modelBuilder.RegisterCisTemporalTable<Entities.SalesArrangementParameters>();
        modelBuilder.RegisterCisTemporalTable<Entities.FlowSwitch>();
    }
}
