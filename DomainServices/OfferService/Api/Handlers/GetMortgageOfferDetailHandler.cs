using _OS = DomainServices.OfferService.Contracts;
using Microsoft.EntityFrameworkCore;

namespace DomainServices.OfferService.Api.Handlers;

internal class GetMortgageOfferDetailHandler
    : IRequestHandler<Dto.GetMortgageOfferDetailMediatrRequest, _OS.GetMortgageOfferDetailResponse>
{
    #region Construction

    private readonly Repositories.OfferServiceDbContext _dbContext;

    public GetMortgageOfferDetailHandler(Repositories.OfferServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    #endregion

    public async Task<_OS.GetMortgageOfferDetailResponse> Handle(Dto.GetMortgageOfferDetailMediatrRequest request, CancellationToken cancellation)
    {
        var entity = await _dbContext.Offers
           .AsNoTracking()
           .Where(t => t.OfferId == request.OfferId)
           .FirstOrDefaultAsync(cancellation) ?? throw new CisNotFoundException(13000, $"Offer #{request.OfferId} not found");

        var model = new _OS.GetMortgageOfferDetailResponse
        {
            OfferId = entity.OfferId,
            ResourceProcessId = entity.ResourceProcessId.ToString(),
            Created = new CIS.Infrastructure.gRPC.CisTypes.ModificationStamp(entity),
            BasicParameters = _OS.BasicParameters.Parser.ParseFrom(entity.BasicParametersBin),
            SimulationInputs = _OS.MortgageSimulationInputs.Parser.ParseFrom(entity.SimulationInputsBin),
            SimulationResults = _OS.MortgageSimulationResults.Parser.ParseFrom(entity.SimulationResultsBin),
            AdditionalSimulationResults = _OS.AdditionalMortgageSimulationResults.Parser.ParseFrom(entity.AdditionalSimulationResultsBin)
        };

        return model;
    }
  
}