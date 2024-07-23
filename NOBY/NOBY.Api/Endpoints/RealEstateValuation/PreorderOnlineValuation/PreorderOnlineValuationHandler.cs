using DomainServices.RealEstateValuationService.Clients;

namespace NOBY.Api.Endpoints.RealEstateValuation.PreorderOnlineValuation;

internal sealed class PreorderOnlineValuationHandler(IRealEstateValuationServiceClient _realEstateValuationService)
    : IRequestHandler<RealEstateValuationPreorderOnlineValuationRequest>
{
    public async Task Handle(RealEstateValuationPreorderOnlineValuationRequest request, CancellationToken cancellationToken)
    {
        var revInstance = await _realEstateValuationService.ValidateRealEstateValuationId(request.RealEstateValuationId, false, cancellationToken);
        if ((revInstance.PossibleValuationTypeId is null || !revInstance.PossibleValuationTypeId.Contains((int)RealEstateValuationTypes.Online))
            || revInstance.PreorderId.HasValue)
        {
            throw new NobyValidationException(90032, "RealEstateValuation type not allowed");
        }

        await _realEstateValuationService.PreorderOnlineValuation(new DomainServices.RealEstateValuationService.Contracts.PreorderOnlineValuationRequest
        {
            RealEstateValuationId = request.RealEstateValuationId,
            OnlinePreorderDetails = new()
            {
                BuildingTechnicalStateCode = request.BuildingTechnicalStateCode,
                FlatSchemaCode = request.FlatSchemaCode,
                BuildingAgeCode = request.BuildingAgeCode,
                BuildingMaterialStructureCode = request.BuildingMaterialStructureCode,
                FlatArea = request.FlatArea
            }
        }, cancellationToken);
    }
}
