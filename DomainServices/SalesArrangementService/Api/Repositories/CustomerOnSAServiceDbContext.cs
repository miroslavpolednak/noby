using Microsoft.EntityFrameworkCore;
using CIS.Infrastructure.Data;
using Microsoft.VisualBasic;

namespace DomainServices.SalesArrangementService.Api.Repositories;

internal sealed class CustomerOnSAServiceDbContext 
    : BaseDbContext<CustomerOnSAServiceDbContext>
{
#pragma warning disable CS8618
    public CustomerOnSAServiceDbContext(BaseDbContextAggregate<CustomerOnSAServiceDbContext> aggregate)
#pragma warning restore CS8618
        : base(aggregate) { }

    public DbSet<Entities.CustomerOnSA> Customers { get; set; }
    public DbSet<Entities.CustomerOnSAIdentity> CustomersIdentities { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.RegisterCisTemporalTable<Entities.CustomerOnSA>();
        //modelBuilder.RegisterCisTemporalTable<Entities.SalesArrangement>();
        
        modelBuilder.Entity<Entities.CustomerOnSA>()
            .HasMany(t => t.Identities)
            .WithOne(t => t.Customer)
            .HasForeignKey(t => t.CustomerOnSAId);
        
    }
}
