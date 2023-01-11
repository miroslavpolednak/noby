using Microsoft.EntityFrameworkCore;
using CIS.Infrastructure.Data;

namespace DomainServices.ProductService.Api.Database;

internal sealed class ProductServiceDbContext 
    : BaseDbContext<ProductServiceDbContext>
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public ProductServiceDbContext(BaseDbContextAggregate<ProductServiceDbContext> aggregate)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        : base(aggregate) { }

    public DbSet<Entities.Loan> Loans { get; set; }
    public DbSet<Entities.Relationship> Relationships { get; set; }
	public DbSet<Entities.Partner> Partners { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Entities.Relationship>()
            .HasOne(t => t.Partner)
            .WithMany()
            .HasForeignKey(t => t.PartnerId);
    }
}
