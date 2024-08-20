using DomainServices.RealEstateValuationService.Clients;

namespace NOBY.Api.Endpoints.RealEstateValuation.SaveLocalSurveyDetails;

internal sealed class SaveLocalSurveyDetailsHandler(IRealEstateValuationServiceClient _realEstateValuationService)
    : IRequestHandler<RealEstateValuationSaveLocalSurveyDetailsRequest>
{
    public async Task Handle(RealEstateValuationSaveLocalSurveyDetailsRequest request, CancellationToken cancellationToken)
    {
        var revDetail = await _realEstateValuationService.GetRealEstateValuationDetail(request.RealEstateValuationId, cancellationToken);

        if (revDetail.ValuationTypeId != DomainServices.RealEstateValuationService.Contracts.ValuationTypes.Online && revDetail.ValuationTypeId != DomainServices.RealEstateValuationService.Contracts.ValuationTypes.Standard)
        {
            throw new NobyValidationException(90032, "ValuationTypeId is not Online or Standard");
        }

        if (revDetail.ValuationTypeId == DomainServices.RealEstateValuationService.Contracts.ValuationTypes.Online && revDetail.ValuationStateId != (int)WorkflowTaskStates.DoplneniDokumentu)
        {
            throw new NobyValidationException(90032, "Online is not in state 10");
        }

        if (revDetail.ValuationTypeId == DomainServices.RealEstateValuationService.Contracts.ValuationTypes.Standard && revDetail.ValuationStateId != (int)WorkflowTaskStates.Rozpracovano)
        {
            throw new NobyValidationException(90032, "Standard is not in state 7");
        }

        var dsRequest = new DomainServices.RealEstateValuationService.Contracts.UpdateRealEstateValuationDetailRequest
        {
            RealEstateStateId = revDetail.RealEstateStateId,
            RealEstateValuationId = revDetail.RealEstateValuationId,
            RealEstateSubtypeId = revDetail.RealEstateSubtypeId,
            Address = revDetail.Address,
            HouseAndFlatDetails = revDetail.HouseAndFlatDetails,
            IsLoanRealEstate = revDetail.IsLoanRealEstate,
            LoanPurposeDetails = revDetail.LoanPurposeDetails,
            ParcelDetails = revDetail.ParcelDetails,
            OnlinePreorderDetails = revDetail.OnlinePreorderDetails,
            Comment = request.Comment ?? "",
            LocalSurveyDetails = new DomainServices.RealEstateValuationService.Contracts.LocalSurveyData
            {
                RealEstateValuationLocalSurveyFunctionCode = request.FunctionCode ?? "",
                Email = request.EmailAddress?.EmailAddress ?? "",
                FirstName = request.FirstName ?? "",
                LastName = request.LastName ?? "",
                PhoneIDC = request.MobilePhone?.PhoneIDC ?? "",
                PhoneNumber = request.MobilePhone?.PhoneNumber ?? ""
            }
        };

        await _realEstateValuationService.UpdateRealEstateValuationDetail(dsRequest, cancellationToken);
    }
}
