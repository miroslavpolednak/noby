using Microsoft.EntityFrameworkCore;
using CIS.Infrastructure.Data;
using Microsoft.VisualBasic;

namespace DomainServices.SalesArrangementService.Api.Repositories;

internal sealed class SalesArrangementServiceDbContext 
    : BaseDbContext<SalesArrangementServiceDbContext>
{
#pragma warning disable CS8618
    public SalesArrangementServiceDbContext(BaseDbContextAggregate<SalesArrangementServiceDbContext> aggregate)
#pragma warning restore CS8618
        : base(aggregate) { }

    public DbSet<Entities.SalesArrangement> SalesArrangements { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.RegisterCisTemporalTable<Entities.SalesArrangement>();
    }
}
