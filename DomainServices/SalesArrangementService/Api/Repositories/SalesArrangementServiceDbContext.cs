using Microsoft.EntityFrameworkCore;
using CIS.Infrastructure.Data;

namespace DomainServices.SalesArrangementService.Api.Repositories;

internal sealed class SalesArrangementServiceDbContext 
    : BaseDbContext<SalesArrangementServiceDbContext>
{
#pragma warning disable CS8618
    public SalesArrangementServiceDbContext(BaseDbContextAggregate<SalesArrangementServiceDbContext> aggregate)
#pragma warning restore CS8618
        : base(aggregate) { }

    public DbSet<Entities.SalesArrangement> SalesArrangements { get; set; }
    public DbSet<Entities.SalesArrangementParameters> SalesArrangementsParameters { get; set; }
    public DbSet<Entities.CustomerOnSA> Customers { get; set; }
    public DbSet<Entities.CustomerIncome> CustomersIncomes { get; set; }
    public DbSet<Entities.CustomerOnSAObligations> CustomersObligations { get; set; }
    public DbSet<Entities.CustomerOnSAIdentity> CustomersIdentities { get; set; }
    public DbSet<Entities.Household> Households { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.RegisterCisTemporalTable<Entities.SalesArrangement>();
        modelBuilder.RegisterCisTemporalTable<Entities.CustomerOnSAObligations>();
        modelBuilder.RegisterCisTemporalTable<Entities.CustomerOnSA>();
        modelBuilder.RegisterCisTemporalTable<Entities.CustomerOnSAIdentity>();
        modelBuilder.RegisterCisTemporalTable<Entities.Household>();
        modelBuilder.RegisterCisTemporalTable<Entities.SalesArrangementParameters>();

        modelBuilder.Entity<Entities.CustomerOnSA>()
            .HasMany(t => t.Identities)
            .WithOne(t => t.Customer)
            .HasForeignKey(t => t.CustomerOnSAId);
    }
}
