using Microsoft.EntityFrameworkCore;
using CIS.Infrastructure.Data;

namespace DomainServices.ProductService.Api.Repositories;

internal sealed class ProductServiceDbContext : BaseDbContext
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public ProductServiceDbContext(DbContextOptions<ProductServiceDbContext> options, CIS.Core.Security.ICurrentUserAccessor userProvider, CIS.Core.IDateTime dateTime)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        : base(options, userProvider, dateTime) { }

    public DbSet<Entities.Loan> Loans { get; set; }
    public DbSet<Entities.Relationship> Relationships { get; set; }
	public DbSet<Entities.Partner> Partners { get; set; }
}
