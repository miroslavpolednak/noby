using Microsoft.EntityFrameworkCore;
using CIS.Infrastructure.Data;

namespace DomainServices.CaseService.Api.Repositories;

internal sealed class CaseServiceDbContext 
    : BaseDbContext<CaseServiceDbContext>
{
    public CaseServiceDbContext(BaseDbContextAggregate<CaseServiceDbContext> aggregate)
        : base(aggregate) { }

    public DbSet<Entities.Case> Cases { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.RegisterCisTemporalTable<Entities.Case>();
    }
}
