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

    public async Task<Entities.Offer> SaveOffer(Guid resourceProcessId,object basicParameters, object simulationInputs, object simulationResults, CancellationToken cancellation)
    {
        var entity = new Entities.Offer
        {
            ResourceProcessId = resourceProcessId,
            BasicParameters = JsonSerializer.Serialize(basicParameters),
            SimulationInputs = JsonSerializer.Serialize(simulationInputs),
            SimulationResults = JsonSerializer.Serialize(simulationResults),
        };

        _dbContext.Offers.Add(entity);

        await _dbContext.SaveChangesAsync(cancellation);

        return entity;
    }

    public async Task<Entities.Offer> Get(int offerId, CancellationToken cancellation)
        => await _dbContext.Offers
           .AsNoTracking()
           .Where(t => t.OfferId == offerId)
           .FirstOrDefaultAsync(cancellation) ?? throw new CisNotFoundException(13000, $"Offer #{offerId} not found");

}