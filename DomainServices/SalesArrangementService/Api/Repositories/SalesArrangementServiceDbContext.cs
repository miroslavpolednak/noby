using Microsoft.EntityFrameworkCore;
using CIS.Infrastructure.Data;

namespace DomainServices.SalesArrangementService.Api.Repositories;

internal sealed class SalesArrangementServiceDbContext : BaseDbContext
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public SalesArrangementServiceDbContext(DbContextOptions<SalesArrangementServiceDbContext> options, CIS.Core.Security.ICurrentUserAccessor userProvider)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        : base(options, userProvider) { }

    public DbSet<Entities.SalesArrangement> SalesArrangements { get; set; }
    public DbSet<Entities.SalesArrangementData> SalesArrangementsData { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.RegisterCisTemporalTable<Entities.SalesArrangement>();
        modelBuilder.RegisterCisTemporalTable<Entities.SalesArrangementData>();
    }
}
