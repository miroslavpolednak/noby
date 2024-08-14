using DomainServices.RealEstateValuationService.Clients;

namespace NOBY.Api.Endpoints.RealEstateValuation.OrderRealEstateValuation;

internal sealed class OrderRealEstateValuationHandler(IRealEstateValuationServiceClient _realEstateValuationService)
    : IRequestHandler<RealEstateValuationOrderRealEstateValuationRequest>
{
    public async Task Handle(RealEstateValuationOrderRealEstateValuationRequest request, CancellationToken cancellationToken)
    {
        var revInstance = await _realEstateValuationService.GetRealEstateValuationDetail(request.RealEstateValuationId, cancellationToken);

        if (!(revInstance.PossibleValuationTypeId?.Contains((int)request.ValuationTypeId) ?? false))
        {
            throw new NobyValidationException(90032, "PossibleValuationTypeId does not contain ValuationTypeId");
        }
        
        switch (request.ValuationTypeId)
        {
            case EnumRealEstateValuationTypes.Online:
                if (!revInstance.PreorderId.HasValue 
                    || revInstance.OrderId.HasValue 
                    || revInstance.ValuationStateId != (int)RealEstateValuationStates.DoplneniDokumentu)
                {
                    throw new NobyValidationException(90032, "Valuation:Online OrderId or PreorderId already set or state is out of allowed range");
                }

                if ((revInstance.Attachments?.Count ?? 0) == 0)
                {
                    throw new NobyValidationException(90065);
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

            case EnumRealEstateValuationTypes.Standard:
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
                    Comment = request.LocalSurveyPerson?.Comment ?? "",
                    LocalSurveyDetails = createData(request),
                }, cancellationToken);
                break;

            case EnumRealEstateValuationTypes.Dts:
                if (revInstance.OrderId.HasValue 
                    || revInstance.ValuationStateId != (int)RealEstateValuationStates.Rozpracovano)
                {
                    throw new NobyValidationException(90032, "Valuation:Dts OrderId already set or state is out of allowed range");
                }

                if ((revInstance.Attachments?.Count ?? 0) == 0)
                {
                    throw new NobyValidationException(90064);
                }

                await _realEstateValuationService.OrderDTSValuation(new DomainServices.RealEstateValuationService.Contracts.OrderDTSValuationRequest
                {
                    RealEstateValuationId = request.RealEstateValuationId,
                    Comment = request.LocalSurveyPerson?.Comment ?? ""
                }, cancellationToken);
                break;
        }
    }

    private static void validateLocalSurvey(RealEstateValuationSharedLocalSurveyData? localSurvey)
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

    private static DomainServices.RealEstateValuationService.Contracts.LocalSurveyData createData(RealEstateValuationOrderRealEstateValuationRequest request)
        => new()
        {
            RealEstateValuationLocalSurveyFunctionCode = request.LocalSurveyPerson?.FunctionCode ?? "",
            FirstName = request.LocalSurveyPerson?.FirstName ?? "",
            LastName = request.LocalSurveyPerson?.LastName ?? "",
            Email = request.LocalSurveyPerson?.EmailAddress?.EmailAddress ?? "",
            PhoneIDC = request.LocalSurveyPerson?.MobilePhone?.PhoneIDC ?? "",
            PhoneNumber = request.LocalSurveyPerson?.MobilePhone?.PhoneNumber ?? ""
        };
}
