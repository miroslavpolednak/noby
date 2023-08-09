using DomainServices.CaseService.Clients;
using DomainServices.RealEstateValuationService.Clients;

namespace NOBY.Api.Endpoints.RealEstateValuation.OrderRealEstateValuation;

internal sealed class OrderRealEstateValuationHandler
    : IRequestHandler<OrderRealEstateValuationRequest>
{
    public async Task Handle(OrderRealEstateValuationRequest request, CancellationToken cancellationToken)
    {

        var revInstance = await _realEstateValuationService.ValidateRealEstateValuationId(request.RealEstateValuationId, false, cancellationToken);
        var caseInstance = await _caseService.ValidateCaseId(request.CaseId, false, cancellationToken);

    }

    private readonly ICaseServiceClient _caseService;
    private readonly IRealEstateValuationServiceClient _realEstateValuationService;
    private readonly Services.RealEstateValuationType.RealEstateValuationTypeService _estateValuationTypeService;

    public OrderRealEstateValuationHandler(
        Services.RealEstateValuationType.RealEstateValuationTypeService estateValuationTypeService,
        ICaseServiceClient caseService,
        IRealEstateValuationServiceClient realEstateValuationService)
    {
        _estateValuationTypeService = estateValuationTypeService;
        _caseService = caseService;
        _realEstateValuationService = realEstateValuationService;
    }
}
