using Microsoft.EntityFrameworkCore;
using CIS.Infrastructure.Data;

namespace DomainServices.OfferService.Api.Database;

internal sealed class OfferServiceDbContext 
    : BaseDbContext<OfferServiceDbContext>
{
    public OfferServiceDbContext(BaseDbContextAggregate<OfferServiceDbContext> aggregate)
        : base(aggregate) { }

    public DbSet<Entities.Offer> Offers { get; set; }
}
