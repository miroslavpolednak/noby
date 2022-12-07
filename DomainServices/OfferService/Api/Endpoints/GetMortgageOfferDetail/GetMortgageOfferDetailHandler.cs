using DomainServices.OfferService.Contracts;
using Microsoft.EntityFrameworkCore;

namespace DomainServices.OfferService.Api.Endpoints.GetMortgageOfferDetail;

internal class GetMortgageOfferDetailHandler
    : IRequestHandler<GetMortgageOfferDetailRequest, GetMortgageOfferDetailResponse>
{
    #region Construction

    private readonly Repositories.OfferServiceDbContext _dbContext;

    public GetMortgageOfferDetailHandler(Repositories.OfferServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    #endregion

    public async Task<GetMortgageOfferDetailResponse> Handle(GetMortgageOfferDetailRequest request, CancellationToken cancellation)
    {
        var entity = await _dbContext.Offers
           .AsNoTracking()
           .Where(t => t.OfferId == request.OfferId)
           .FirstOrDefaultAsync(cancellation) ?? throw new CisNotFoundException(10000, $"Offer #{request.OfferId} not found");

        var model = new GetMortgageOfferDetailResponse
        {
            OfferId = entity.OfferId,
            ResourceProcessId = entity.ResourceProcessId.ToString(),
            Created = new CIS.Infrastructure.gRPC.CisTypes.ModificationStamp(entity),
            BasicParameters = BasicParameters.Parser.ParseFrom(entity.BasicParametersBin),
            SimulationInputs = MortgageSimulationInputs.Parser.ParseFrom(entity.SimulationInputsBin),
            SimulationResults = MortgageSimulationResults.Parser.ParseFrom(entity.SimulationResultsBin),
            AdditionalSimulationResults = AdditionalMortgageSimulationResults.Parser.ParseFrom(entity.AdditionalSimulationResultsBin)
        };

        return model;
    }

}