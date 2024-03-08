using CIS.Infrastructure.Data;
using DomainServices.CaseService.Api.Database.Entities;

namespace DomainServices.CaseService.Api.Database;

internal sealed class CaseServiceDbContext
    : BaseDbContext<CaseServiceDbContext>
{
    public CaseServiceDbContext(BaseDbContextAggregate<CaseServiceDbContext> aggregate)
        : base(aggregate) { }

    public DbSet<Case> Cases { get; set; }
    public DbSet<ActiveTask> ActiveTasks { get; set; }
    public DbSet<ConfirmedPriceException> ConfirmedPriceExceptions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.RegisterCisTemporalTable<Case>();
        modelBuilder.RegisterCisTemporalTable<ActiveTask>();
    }
}
