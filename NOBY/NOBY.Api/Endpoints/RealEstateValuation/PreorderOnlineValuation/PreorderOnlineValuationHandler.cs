using CIS.Foms.Enums;
using DomainServices.RealEstateValuationService.Clients;

namespace NOBY.Api.Endpoints.RealEstateValuation.PreorderOnlineValuation;

internal sealed class PreorderOnlineValuationHandler
    : IRequestHandler<PreorderOnlineValuationRequest>
{
    public async Task Handle(PreorderOnlineValuationRequest request, CancellationToken cancellationToken)
    {
        var allowedTypes = await _estateValuationTypeService.GetAllowedTypes(request.RealEstateValuationId, request.CaseId, cancellationToken);
        if (!allowedTypes.Contains(RealEstateValuationTypes.Online))
        {
            throw new NobyValidationException(90032, "RealEstateValuation type not allowed");
        }

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

    private readonly Services.RealEstateValuationType.IRealEstateValuationTypeService _estateValuationTypeService;
    private readonly IRealEstateValuationServiceClient _realEstateValuationService;

    public PreorderOnlineValuationHandler(IRealEstateValuationServiceClient realEstateValuationService, Services.RealEstateValuationType.IRealEstateValuationTypeService estateValuationTypeService)
    {
        _realEstateValuationService = realEstateValuationService;
        _estateValuationTypeService = estateValuationTypeService;
    }
}
