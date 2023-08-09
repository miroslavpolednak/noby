using DomainServices.RealEstateValuationService.Clients;

namespace NOBY.Api.Endpoints.RealEstateValuation.PreorderOnlineValuation;

internal sealed class PreorderOnlineValuationHandler
    : IRequestHandler<PreorderOnlineValuationRequest>
{
    public async Task Handle(PreorderOnlineValuationRequest request, CancellationToken cancellationToken)
    {
        await _realEstateValuationService.PreorderOnlineValuation(new DomainServices.RealEstateValuationService.Contracts.PreorderOnlineValuationRequest
        {
            RealEstateValuationId = request.RealEstateValuationId,
            Data = new DomainServices.RealEstateValuationService.Contracts.OrdersOnlinePreorder
            {
                BuildingTechnicalStateCode = request.BuildingTechnicalStateCode,
                FlatSchemaCode = request.FlatSchemaCode,
                BuildingAgeCode = request.BuildingAgeCode,
                BuildingMaterialStructureCode = request.BuildingMaterialStructureCode,
                FlatArea = request.FlatArea
            }
        }, cancellationToken);
    }

    private readonly IRealEstateValuationServiceClient _realEstateValuationService;

    public PreorderOnlineValuationHandler(IRealEstateValuationServiceClient realEstateValuationService)
    {
        _realEstateValuationService = realEstateValuationService;
    }
}
