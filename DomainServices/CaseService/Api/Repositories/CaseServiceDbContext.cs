using Microsoft.EntityFrameworkCore;
using CIS.Infrastructure.Data;

namespace DomainServices.CaseService.Api.Repositories;

internal sealed class CaseServiceDbContext : BaseDbContext
{
    public CaseServiceDbContext(BaseDbContextAggregate aggregate)
        : base(aggregate) { }

    public DbSet<Entities.Case> Cases { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.RegisterCisTemporalTable<Entities.Case>();
    }
}
