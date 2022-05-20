using Microsoft.EntityFrameworkCore;
using CIS.Infrastructure.Data;

namespace DomainServices.CaseService.Api.Repositories;

internal sealed class CaseServiceDbContext 
    : BaseDbContext<CaseServiceDbContext>
{
    public CaseServiceDbContext(BaseDbContextAggregate<CaseServiceDbContext> aggregate)
        : base(aggregate) { }

    public DbSet<Entities.Case> Cases { get; set; } = null!;
    public DbSet<Entities.CaseActiveTasks> CaseActiveTasks { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.RegisterCisTemporalTable<Entities.Case>();
        modelBuilder.RegisterCisTemporalTable<Entities.CaseActiveTasks>();

        modelBuilder.Entity<Entities.Case>()
            .HasOne(t => t.ActiveTasks)
            .WithOne(t => t.ParentCase)
            .HasForeignKey<Entities.CaseActiveTasks>(t => t.CaseId);
    }
}
