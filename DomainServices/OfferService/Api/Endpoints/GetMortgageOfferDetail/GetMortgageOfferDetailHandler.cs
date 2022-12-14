using DomainServices.OfferService.Contracts;
using Microsoft.EntityFrameworkCore;

namespace DomainServices.OfferService.Api.Endpoints.GetMortgageOfferDetail;

internal sealed class GetMortgageOfferDetailHandler
    : IRequestHandler<GetMortgageOfferDetailRequest, GetMortgageOfferDetailResponse>
{
    public async Task<GetMortgageOfferDetailResponse> Handle(GetMortgageOfferDetailRequest request, CancellationToken cancellationToken)
    {
        var entity = await _dbContext.Offers
           .AsNoTracking()
           .Where(t => t.OfferId == request.OfferId)
           .FirstOrDefaultAsync(cancellationToken) ?? throw new CisNotFoundException(10000, $"Offer #{request.OfferId} not found");

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
    
    private readonly Database.OfferServiceDbContext _dbContext;

    public GetMortgageOfferDetailHandler(Database.OfferServiceDbContext dbContext)
        => _dbContext = dbContext;
}