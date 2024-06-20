using DomainServices.RealEstateValuationService.Clients;

namespace NOBY.Api.Endpoints.RealEstateValuation.OrderRealEstateValuation;

internal sealed class OrderRealEstateValuationHandler(IRealEstateValuationServiceClient _realEstateValuationService)
        : IRequestHandler<OrderRealEstateValuationRequest>
{
    public async Task Handle(OrderRealEstateValuationRequest request, CancellationToken cancellationToken)
    {
        var revInstance = await _realEstateValuationService.GetRealEstateValuationDetail(request.RealEstateValuationId, cancellationToken);

        if (!(revInstance.PossibleValuationTypeId?.Contains((int)request.ValuationTypeId) ?? false))
        {
            throw new NobyValidationException(90032, "PossibleValuationTypeId does not contain ValuationTypeId");
        }

        switch (request.ValuationTypeId)
        {
            case RealEstateValuationTypes.Online:
                if (!revInstance.PreorderId.HasValue 
                    || revInstance.OrderId.HasValue 
                    || revInstance.ValuationStateId != (int)RealEstateValuationStates.DoplneniDokumentu)
                {
                    throw new NobyValidationException(90032, "Valuation:Online OrderId or PreorderId already set or state is out of allowed range");
                }

                if (revInstance.IsRevaluationRequired)
                {
                    validateLocalSurvey(request.LocalSurveyPerson);
                }

                await _realEstateValuationService.OrderOnlineValuation(new DomainServices.RealEstateValuationService.Contracts.OrderOnlineValuationRequest
                {
                    RealEstateValuationId = request.RealEstateValuationId,
                    LocalSurveyDetails = createData(request),
                }, cancellationToken);
                break;

            case RealEstateValuationTypes.Standard:
                if (revInstance.OrderId.HasValue 
                    || !(new[] { (int)RealEstateValuationStates.DoplneniDokumentu, (int)RealEstateValuationStates.Rozpracovano }).Contains(revInstance.ValuationStateId))
                {
                    throw new NobyValidationException(90032, "Valuation:Standard OrderId already set or state is out of allowed range");
                }

                if (revInstance.IsRevaluationRequired)
                {
                    validateLocalSurvey(request.LocalSurveyPerson);
                }

                await _realEstateValuationService.OrderStandardValuation(new DomainServices.RealEstateValuationService.Contracts.OrderStandardValuationRequest
                {
                    RealEstateValuationId = request.RealEstateValuationId,
                    LocalSurveyDetails = createData(request),
                }, cancellationToken);
                break;

            case RealEstateValuationTypes.Dts:
                if (revInstance.OrderId.HasValue 
                    || revInstance.ValuationStateId != (int)RealEstateValuationStates.Rozpracovano)
                {
                    throw new NobyValidationException(90032, "Valuation:Dts OrderId already set or state is out of allowed range");
                }

                await _realEstateValuationService.OrderDTSValuation(request.RealEstateValuationId, cancellationToken);
                break;
        }
    }

    public static void validateLocalSurvey(Dto.RealEstateValuation.LocalSurveyData? localSurvey)
    {
        if (string.IsNullOrEmpty(localSurvey?.FunctionCode)
            || string.IsNullOrEmpty(localSurvey?.FirstName)
            || string.IsNullOrEmpty(localSurvey?.LastName)
            || string.IsNullOrEmpty(localSurvey?.MobilePhone?.PhoneIDC)
            || string.IsNullOrEmpty(localSurvey?.MobilePhone?.PhoneNumber)
            || string.IsNullOrEmpty(localSurvey?.EmailAddress?.EmailAddress))
        {
            throw new NobyValidationException(90032, "LocalSurveyPerson is not filled");
        }
    }

    private static DomainServices.RealEstateValuationService.Contracts.LocalSurveyData createData(OrderRealEstateValuationRequest request)
        => new DomainServices.RealEstateValuationService.Contracts.LocalSurveyData
        {
            RealEstateValuationLocalSurveyFunctionCode = request.LocalSurveyPerson?.FunctionCode ?? "",
            FirstName = request.LocalSurveyPerson?.FirstName ?? "",
            LastName = request.LocalSurveyPerson?.LastName ?? "",
            Email = request.LocalSurveyPerson?.EmailAddress?.EmailAddress ?? "",
            PhoneIDC = request.LocalSurveyPerson?.MobilePhone?.PhoneIDC ?? "",
            PhoneNumber = request.LocalSurveyPerson?.MobilePhone?.PhoneNumber ?? ""
        };
}
