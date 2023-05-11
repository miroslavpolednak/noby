using DomainServices.OfferService.Contracts;
using Microsoft.EntityFrameworkCore;

namespace DomainServices.OfferService.Api.Endpoints.GetMortgageOfferDetail;

internal sealed class GetMortgageOfferDetailHandler
    : IRequestHandler<GetMortgageOfferDetailRequest, GetMortgageOfferDetailResponse>
{
    public async Task<GetMortgageOfferDetailResponse> Handle(GetMortgageOfferDetailRequest request, CancellationToken cancellationToken)
    {
        var entity = await _dbContext
            .Offers
            .AsNoTracking()
            .Where(t => t.OfferId == request.OfferId)
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.OfferNotFound, request.OfferId);

        var model = new GetMortgageOfferDetailResponse
        {
            OfferId = entity.OfferId,
            ResourceProcessId = entity.ResourceProcessId.ToString(),
            Created = new CIS.Infrastructure.gRPC.CisTypes.ModificationStamp(entity),
            BasicParameters = BasicParameters.Parser.ParseFrom(entity.BasicParametersBin),
            SimulationInputs = MortgageSimulationInputs.Parser.ParseFrom(entity.SimulationInputsBin),
            SimulationResults = MortgageSimulationResults.Parser.ParseFrom(entity.SimulationResultsBin),
            AdditionalSimulationResults = AdditionalMortgageSimulationResults.Parser.ParseFrom(entity.AdditionalSimulationResultsBin),
            IsCreditWorthinessSimpleRequested = entity.IsCreditWorthinessSimpleRequested,
            CreditWorthinessSimpleInputs = entity.CreditWorthinessSimpleInputsBin is null ? null : MortgageCreditWorthinessSimpleInputs.Parser.ParseFrom(entity.CreditWorthinessSimpleInputsBin)
        };

        return model;
    }
    
    private readonly Database.OfferServiceDbContext _dbContext;

    public GetMortgageOfferDetailHandler(Database.OfferServiceDbContext dbContext)
        => _dbContext = dbContext;
}