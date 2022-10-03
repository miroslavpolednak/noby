using Microsoft.EntityFrameworkCore;
using CIS.Infrastructure.Data;

namespace DomainServices.HouseholdService.Api.Repositories;

internal sealed class HouseholdServiceDbContext
    : BaseDbContext<HouseholdServiceDbContext>
{
#pragma warning disable CS8618
    public HouseholdServiceDbContext(BaseDbContextAggregate<HouseholdServiceDbContext> aggregate)
#pragma warning restore CS8618
        : base(aggregate) { }

    public DbSet<Entities.CustomerOnSA> Customers { get; set; }
    public DbSet<Entities.CustomerOnSAIncome> CustomersIncomes { get; set; }
    public DbSet<Entities.CustomerOnSAObligation> CustomersObligations { get; set; }
    public DbSet<Entities.CustomerOnSAIdentity> CustomersIdentities { get; set; }
    public DbSet<Entities.Household> Households { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.RegisterCisTemporalTable<Entities.CustomerOnSAObligation>();
        modelBuilder.RegisterCisTemporalTable<Entities.CustomerOnSA>();
        modelBuilder.RegisterCisTemporalTable<Entities.CustomerOnSAIdentity>();
        modelBuilder.RegisterCisTemporalTable<Entities.Household>();

        modelBuilder.Entity<Entities.CustomerOnSA>()
            .HasMany(t => t.Identities)
            .WithOne(t => t.Customer)
            .HasForeignKey(t => t.CustomerOnSAId);
    }
}