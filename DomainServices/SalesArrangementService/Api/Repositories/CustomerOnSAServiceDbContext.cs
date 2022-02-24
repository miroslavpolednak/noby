using Microsoft.EntityFrameworkCore;
using CIS.Infrastructure.Data;
using Microsoft.VisualBasic;

namespace DomainServices.SalesArrangementService.Api.Repositories;

internal sealed class CustomerOnSAServiceDbContext : BaseDbContext
{
#pragma warning disable CS8618
    public CustomerOnSAServiceDbContext(BaseDbContextAggregate aggregate)
#pragma warning restore CS8618
        : base(aggregate) { }

    public DbSet<Entities.CustomerOnSA> Customers { get; set; }
    public DbSet<Entities.CustomerOnSAIdentity> CustomersIdentities { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //modelBuilder.RegisterCisTemporalTable<Entities.SalesArrangement>();
        //modelBuilder.RegisterCisTemporalTable<Entities.SalesArrangement>();
        
        modelBuilder.Entity<Entities.CustomerOnSA>()
            .HasMany(t => t.Identities)
            .WithOne(t => t.Customer)
            .HasForeignKey(t => t.CustomerOnSAId);
        
    }
}
