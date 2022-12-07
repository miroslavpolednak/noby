using DomainServices.OfferService.Contracts;
using Microsoft.EntityFrameworkCore;

namespace DomainServices.OfferService.Api.Endpoints.GetMortgageOffer;

internal class GetMortgageOfferHandler
    : IRequestHandler<GetMortgageOfferRequest, GetMortgageOfferResponse>
{
    #region Construction

    private readonly Repositories.OfferServiceDbContext _dbContext;

    public GetMortgageOfferHandler(Repositories.OfferServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    #endregion

    public async Task<GetMortgageOfferResponse> Handle(GetMortgageOfferRequest request, CancellationToken cancellation)
    {
        var entity = await _dbContext.Offers
           .AsNoTracking()
           .Where(t => t.OfferId == request.OfferId)
           .Select(t => new
           {
               t.ResourceProcessId,
               t.CreatedUserId,
               t.CreatedUserName,
               t.CreatedTime,
               t.BasicParametersBin,
               t.SimulationInputsBin,
               t.SimulationResultsBin
           })
           .FirstOrDefaultAsync(cancellation) ?? throw new CisNotFoundException(10000, $"Offer #{request.OfferId} not found");

        var model = new GetMortgageOfferResponse
        {
            OfferId = request.OfferId,
            ResourceProcessId = entity.ResourceProcessId.ToString(),
            Created = new CIS.Infrastructure.gRPC.CisTypes.ModificationStamp(entity.CreatedUserId, entity.CreatedUserName, entity.CreatedTime),
            BasicParameters = BasicParameters.Parser.ParseFrom(entity.BasicParametersBin),
            SimulationInputs = MortgageSimulationInputs.Parser.ParseFrom(entity.SimulationInputsBin),
            SimulationResults = MortgageSimulationResults.Parser.ParseFrom(entity.SimulationResultsBin)
        };

        return model;
    }

}