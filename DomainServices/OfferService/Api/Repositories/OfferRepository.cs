using CIS.Core.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace DomainServices.OfferService.Api.Repositories;

[CIS.Infrastructure.Attributes.ScopedService, CIS.Infrastructure.Attributes.SelfService]
internal class OfferRepository
{
    private readonly OfferServiceDbContext _dbContext;

    public OfferRepository(OfferServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<int> SaveOffer(Guid resourceProcessId, int ProductInstanceTypeId, object inputs, object outputs)
    {
        var entity = new Entities.Offer
        {
            ResourceProcessId = resourceProcessId,
            ProductInstanceTypeId = ProductInstanceTypeId,
            Outputs = JsonSerializer.Serialize(outputs),
            Inputs = JsonSerializer.Serialize(inputs)
        };

        _dbContext.Offers.Add(entity);

        await _dbContext.SaveChangesAsync();

        return entity.OfferId;
    }

    public async Task<Entities.Offer> Get(int offerId)
        => await _dbContext.Offers
           .AsNoTracking()
           .Where(t => t.OfferId == offerId)
           .FirstOrDefaultAsync() ?? throw new CisNotFoundException(13000, $"Offer #{offerId} not found");

    public async Task<bool> AnyOfResourceProcessId(Guid resourceProcessId)
       => await _dbContext.Offers
          .AsNoTracking()
          .Where(t => t.ResourceProcessId == resourceProcessId)
          .AnyAsync();
}

