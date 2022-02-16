using Microsoft.EntityFrameworkCore;
using CIS.Infrastructure.Data;

namespace DomainServices.OfferService.Api.Repositories;

internal sealed class OfferServiceDbContext : BaseDbContext
{
    public OfferServiceDbContext(BaseDbContextAggregate aggregate)
        : base(aggregate) { }

    public DbSet<Entities.Offer> Offers { get; set; }
}
