using Microsoft.EntityFrameworkCore;
using CIS.Infrastructure.Data;

namespace DomainServices.SalesArrangementService.Api.Database;

internal sealed class NobyDbContext
    : BaseDbContext<NobyDbContext>
{
#pragma warning disable CS8618
    public NobyDbContext(BaseDbContextAggregate<NobyDbContext> aggregate)
#pragma warning restore CS8618
        : base(aggregate) { }

    public DbSet<Entities.FormInstanceInterface> FormInstanceInterfaces { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Call 'HasNoKey' in 'OnModelCreating'
        // Unable to track an instance of type 'FormInstanceInterface' because it does not have a primary key. Only entity types with a primary key may be tracked.

        // modelBuilder.Entity<Entities.FormInstanceInterface>().HasNoKey();
    }
}
