using DomainServices.RealEstateValuationService.Clients;

namespace NOBY.Api.Endpoints.RealEstateValuation.SaveOnlinePreorderDetails;

internal sealed class SaveOnlinePreorderDetailsHandler(IRealEstateValuationServiceClient _realEstateValuationService)
    : IRequestHandler<RealEstateValuationSaveOnlinePreorderDetailsRequest>
{
    public async Task Handle(RealEstateValuationSaveOnlinePreorderDetailsRequest request, CancellationToken cancellationToken)
    {
        var revDetail = await _realEstateValuationService.GetRealEstateValuationDetail(request.RealEstateValuationId, cancellationToken);

        if (revDetail.ValuationTypeId != DomainServices.RealEstateValuationService.Contracts.ValuationTypes.Online)
        {
            throw new NobyValidationException(90032, "ValuationTypeId is not Online");
        }

        if (revDetail.ValuationStateId != (int)WorkflowTaskStates.Rozpracovano)
        {
            throw new NobyValidationException(90032, "Online is not in state 7");
        }

        if (!(revDetail.PossibleValuationTypeId?.Contains(1) ?? false))
        {
            throw new NobyValidationException(90032, "PossibleValuationTypeId does not contain 1");
        }

        if (revDetail.PreorderId.HasValue)
        {
            throw new NobyValidationException(90032, "PreorderId is not null");
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
            LocalSurveyDetails = revDetail.LocalSurveyDetails,
            OnlinePreorderDetails = new DomainServices.RealEstateValuationService.Contracts.OnlinePreorderData
            {
                BuildingMaterialStructureCode = request.BuildingMaterialStructureCode,
                BuildingTechnicalStateCode = request.BuildingTechnicalStateCode,
                FlatSchemaCode = request.FlatSchemaCode,
                BuildingAgeCode = request.BuildingAgeCode,
                FlatArea = request.FlatArea
            }
        };

        await _realEstateValuationService.UpdateRealEstateValuationDetail(dsRequest, cancellationToken);
    }
}
