using _OS = DomainServices.OfferService.Contracts;
using Microsoft.EntityFrameworkCore;

namespace DomainServices.OfferService.Api.Handlers;

internal class GetMortgageOfferHandler
    : IRequestHandler<Dto.GetMortgageOfferMediatrRequest, _OS.GetMortgageOfferResponse>
{
    #region Construction

    private readonly Repositories.OfferServiceDbContext _dbContext;

    public GetMortgageOfferHandler(Repositories.OfferServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    #endregion

    public async Task<_OS.GetMortgageOfferResponse> Handle(Dto.GetMortgageOfferMediatrRequest request, CancellationToken cancellation)
    {
        var entity = await _dbContext.Offers
           .AsNoTracking()
           .Where(t => t.OfferId == request.OfferId)
           .Select(t => new {
               ResourceProcessId = t.ResourceProcessId,
               CreatedUserId = t.CreatedUserId,
               CreatedUserName = t.CreatedUserName,
               CreatedTime = t.CreatedTime,
               BasicParametersBin = t.BasicParametersBin,
               SimulationInputsBin = t.SimulationInputsBin,
               SimulationResultsBin = t.SimulationResultsBin
           })
           .FirstOrDefaultAsync(cancellation) ?? throw new CisNotFoundException(13000, $"Offer #{request.OfferId} not found");

        var model = new _OS.GetMortgageOfferResponse
        {
            OfferId = request.OfferId,
            ResourceProcessId = entity.ResourceProcessId.ToString(),
            Created = new CIS.Infrastructure.gRPC.CisTypes.ModificationStamp(entity.CreatedUserId, entity.CreatedUserName, entity.CreatedTime),
            BasicParameters = _OS.BasicParameters.Parser.ParseFrom(entity.BasicParametersBin),
            SimulationInputs = _OS.MortgageSimulationInputs.Parser.ParseFrom(entity.SimulationInputsBin),
            SimulationResults = _OS.MortgageSimulationResults.Parser.ParseFrom(entity.SimulationResultsBin)
        };

        return model;
    }
  
}