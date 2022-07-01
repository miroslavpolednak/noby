using Microsoft.EntityFrameworkCore;
using Google.Protobuf;

namespace DomainServices.OfferService.Api.Repositories;

[CIS.Infrastructure.Attributes.ScopedService, CIS.Infrastructure.Attributes.SelfService]
internal class OfferRepository
{
    private readonly OfferServiceDbContext _dbContext;

    public OfferRepository(OfferServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Entities.Offer> SaveOffer(Guid resourceProcessId, IMessage basicParameters, IMessage simulationInputs, IMessage simulationResults, CancellationToken cancellation)
    {
        var entity = new Entities.Offer
        {
            ResourceProcessId = resourceProcessId,
            BasicParameters = Newtonsoft.Json.JsonConvert.SerializeObject(basicParameters),
            SimulationInputs = Newtonsoft.Json.JsonConvert.SerializeObject(simulationInputs),
            SimulationResults = Newtonsoft.Json.JsonConvert.SerializeObject(simulationResults),
            BasicParametersBin = basicParameters.ToByteArray(),
            SimulationInputsBin = simulationInputs.ToByteArray(),
            SimulationResultsBin = simulationResults.ToByteArray()
        };

        _dbContext.Offers.Add(entity);

        await _dbContext.SaveChangesAsync(cancellation);

        return entity;
    }
}