using Microsoft.EntityFrameworkCore;
using CIS.Infrastructure.Data;
using Microsoft.VisualBasic;

namespace DomainServices.SalesArrangementService.Api.Repositories;

internal sealed class SalesArrangementServiceDbContext : BaseDbContext
{
    public SalesArrangementServiceDbContext(BaseDbContextAggregate aggregate)
        : base(aggregate) { }

    public DbSet<Entities.SalesArrangement> SalesArrangements { get; set; }
    public DbSet<Entities.SalesArrangementData> SalesArrangementsData { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.RegisterCisTemporalTable<Entities.SalesArrangement>();
        modelBuilder.RegisterCisTemporalTable<Entities.SalesArrangementData>();
    }
}
