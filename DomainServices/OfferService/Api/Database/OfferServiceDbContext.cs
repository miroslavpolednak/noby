using Microsoft.EntityFrameworkCore;
using CIS.Infrastructure.Data;

namespace DomainServices.OfferService.Api.Database;

internal sealed class OfferServiceDbContext 
    : BaseDbContext<OfferServiceDbContext>
{
    public OfferServiceDbContext(BaseDbContextAggregate<OfferServiceDbContext> aggregate)
        : base(aggregate) { }

    public DbSet<Entities.Offer> Offers { get; set; }
    
    public DbSet<Entities.ResponseCode> ResponseCodes { get; set; }

    public DbSet<Entities.CaseIdAccountNumberKonstDb> CaseIdAccountNumbers { get; set; }

    public DbSet<Entities.ApplicationEvent> ApplicationEvents { get; set; }
    

}
