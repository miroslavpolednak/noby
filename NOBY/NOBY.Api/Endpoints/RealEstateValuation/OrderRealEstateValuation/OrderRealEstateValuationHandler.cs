using CIS.Foms.Enums;
using DomainServices.RealEstateValuationService.Clients;

namespace NOBY.Api.Endpoints.RealEstateValuation.OrderRealEstateValuation;

internal sealed class OrderRealEstateValuationHandler
    : IRequestHandler<OrderRealEstateValuationRequest>
{
    public async Task Handle(OrderRealEstateValuationRequest request, CancellationToken cancellationToken)
    {
        var allowedTypes = await _estateValuationTypeService.GetAllowedTypes(request.RealEstateValuationId, request.CaseId, cancellationToken);
        if (!(allowedTypes?.Contains(request.ValuationTypeId) ?? false))
        {
            throw new CisAuthorizationException("Allowed types is null");
        }

        switch (request.ValuationTypeId)
        {
            case RealEstateValuationTypes.Online:
                await _realEstateValuationService.OrderOnlineValuation(new DomainServices.RealEstateValuationService.Contracts.OrderOnlineValuationRequest
                {
                    RealEstateValuationId = request.RealEstateValuationId,
                    Data = createData(request),
                }, cancellationToken);
                break;

            case RealEstateValuationTypes.Standard:
                await _realEstateValuationService.OrderStandardValuation(new DomainServices.RealEstateValuationService.Contracts.OrderStandardValuationRequest
                {
                    RealEstateValuationId = request.RealEstateValuationId,
                    Data = createData(request),
                }, cancellationToken);
                break;

            case RealEstateValuationTypes.Dts:
                break;
        }
    }

    private static DomainServices.RealEstateValuationService.Contracts.OrdersStandard createData(OrderRealEstateValuationRequest request)
    {
        return new DomainServices.RealEstateValuationService.Contracts.OrdersStandard
        {
            RealEstateValuationLocalSurveyFunctionCode = request.LocalSurveyPerson?.FunctionCode,
            FirstName = request.LocalSurveyPerson?.FirstName,
            LastName = request.LocalSurveyPerson?.LastName,
            Email = request.LocalSurveyPerson?.EmailAddress?.EmailAddress,
            PhoneIDC = request.LocalSurveyPerson?.MobilePhone?.PhoneIDC,
            PhoneNumber = request.LocalSurveyPerson?.MobilePhone?.PhoneNumber
        };
    }

    private readonly IRealEstateValuationServiceClient _realEstateValuationService;
    private readonly Services.RealEstateValuationType.IRealEstateValuationTypeService _estateValuationTypeService;

    public OrderRealEstateValuationHandler(
        Services.RealEstateValuationType.IRealEstateValuationTypeService estateValuationTypeService,
        IRealEstateValuationServiceClient realEstateValuationService)
    {
        _estateValuationTypeService = estateValuationTypeService;
        _realEstateValuationService = realEstateValuationService;
    }
}
