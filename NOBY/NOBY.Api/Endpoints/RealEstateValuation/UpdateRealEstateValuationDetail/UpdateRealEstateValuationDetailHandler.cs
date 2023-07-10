using System.Text.Json;
using CIS.Foms.Enums;
using DomainServices.CaseService.Clients;
using DomainServices.RealEstateValuationService.Clients;
using NOBY.Api.Endpoints.RealEstateValuation.Shared;
using NOBY.Api.Endpoints.RealEstateValuation.Shared.SpecificDetails;
using System.Text.Json.Nodes;
using __Contracts = DomainServices.RealEstateValuationService.Contracts;

namespace NOBY.Api.Endpoints.RealEstateValuation.UpdateRealEstateValuationDetail;

public class UpdateRealEstateValuationDetailHandler : IRequestHandler<UpdateRealEstateValuationDetailRequest>
{
    private readonly ICaseServiceClient _caseService;
    private readonly IRealEstateValuationServiceClient _realEstateValuationService;

    public UpdateRealEstateValuationDetailHandler(ICaseServiceClient caseService, IRealEstateValuationServiceClient realEstateValuationService)
    {
        _caseService = caseService;
        _realEstateValuationService = realEstateValuationService;
    }

    public async Task Handle(UpdateRealEstateValuationDetailRequest request, CancellationToken cancellationToken)
    {
        await CheckIfRequestIsValid(request, cancellationToken);

        var dsRequest = new __Contracts.UpdateRealEstateValuationDetailRequest
        {
            RealEstateValuationId = request.RealEstateValuationId,
            IsLoanRealEstate = request.IsLoanRealEstate,
            RealEstateStateId = request.RealEstateStateId,
            Address = request.Address,
            RealEstateSubtypeId = request.RealEstateSubtypeId,
            LoanPurposeDetails = request.LoanPurposeDetails is null ? null : new __Contracts.LoanPurposeDetailsObject
            {
                LoanPurposes = { request.LoanPurposeDetails.LoanPurposes }
            }
        };

        switch (request.SpecificDetails)
        {
            case HouseAndFlatDetails houseAndFlatDetails:
                dsRequest.HouseAndFlatDetails = new __Contracts.SpecificDetailHouseAndFlatObject
                {
                    PoorCondition = houseAndFlatDetails.PoorCondition,
                    OwnershipRestricted = houseAndFlatDetails.OwnershipRestricted,
                    FlatOnlyDetails = houseAndFlatDetails.FlatOnlyDetails is null ? null : new __Contracts.SpecificDetailFlatOnlyDetails
                    {
                        SpecialPlacement = houseAndFlatDetails.FlatOnlyDetails.SpecialPlacement,
                        Basement = houseAndFlatDetails.FlatOnlyDetails.Basement
                    },
                    FinishedHouseAndFlatDetails = houseAndFlatDetails.FinishedHouseAndFlatDetails is null ? null : new __Contracts.SpecificDetailFinishedHouseAndFlatDetails
                    {
                        Leased = houseAndFlatDetails.FinishedHouseAndFlatDetails.Leased,
                        LeaseApplicable = houseAndFlatDetails.FinishedHouseAndFlatDetails.LeaseApplicable

                    }
                };
                break;
            case ParcelDetails parcelDetails:
                dsRequest.ParcelDetails = new __Contracts.SpecificDetailParcelObject
                {
                    ParcelNumber = parcelDetails.ParcelNumber
                };
                break;
        }

        await _realEstateValuationService.UpdateRealEstateValuationDetail(dsRequest, cancellationToken);
    }

    private async Task CheckIfRequestIsValid(UpdateRealEstateValuationDetailRequest request, CancellationToken cancellationToken)
    {
        var caseInstance = await _caseService.GetCaseDetail(request.CaseId, cancellationToken);
        var valuationDetail = await _realEstateValuationService.GetRealEstateValuationDetail(request.RealEstateValuationId, cancellationToken);

        if (valuationDetail.RealEstateValuationGeneralDetails.CaseId != request.CaseId)
            throw new CisAuthorizationException("The requested RealEstateValuation is not assigned to the requested Case");

        if (valuationDetail.RealEstateValuationGeneralDetails.ValuationStateId != 7)
            throw new CisAuthorizationException("The valuation is not in progress");

        if (caseInstance.State == (int)CaseStates.InProgress && request.LoanPurposeDetails is not null)
            throw new CisAuthorizationException("The LoanPurposeDetails has to be null when the case is in progress");

        var variant = RealEstateVariantHelper.GetRealEstateVariant(valuationDetail.RealEstateValuationGeneralDetails.RealEstateTypeId);

        ParseAndSetSpecificDetails(request, variant);

        switch (variant)
        {
            case RealEstateVariant.HouseAndFlat: CheckHouseAndFlatDetails(request);
                break;

            case RealEstateVariant.OnlyFlat: CheckOnlyFlatDetails(request);
                break;

            case RealEstateVariant.Parcel: CheckParcelDetails(request);
                break;

            case RealEstateVariant.Other: CheckOtherDetails(request);
                break;

            default: throw new NotImplementedException();
        }
    }

    private static void ParseAndSetSpecificDetails(UpdateRealEstateValuationDetailRequest request, RealEstateVariant variant)
    {
        if (request.SpecificDetails is not JsonElement jsonElement)
            return;

        request.SpecificDetails = variant switch
        {
            RealEstateVariant.HouseAndFlat or RealEstateVariant.OnlyFlat => jsonElement.Deserialize<HouseAndFlatDetails>(),
            RealEstateVariant.Parcel => jsonElement.Deserialize<ParcelDetails>(),
            _ => default
        };
    }

    private static void CheckHouseAndFlatDetails(UpdateRealEstateValuationDetailRequest request)
    {
        if (request.SpecificDetails is not HouseAndFlatDetails houseAndFlatDetails)
            throw new CisAuthorizationException("SpecificDetails does not contain the HouseAndFlatDetails object");

        if (houseAndFlatDetails.FinishedHouseAndFlatDetails is null)
        {
            if (request.RealEstateStateId is 1)
                throw new CisAuthorizationException("The RealEstate StateId has invalid value or the FinishedHouseAndFlatDetails object is invalid");
        }
        else
        {
            if (request.RealEstateStateId is not 1)
                throw new CisAuthorizationException("The RealEstate StateId has invalid value or the FinishedHouseAndFlatDetails object is invalid");
        }
    }

    private static void CheckOnlyFlatDetails(UpdateRealEstateValuationDetailRequest request)
    {
        CheckHouseAndFlatDetails(request);

        if ((request.SpecificDetails as HouseAndFlatDetails)?.FlatOnlyDetails is null)
            throw new CisAuthorizationException("The FlatOnlyDetails is required");
    }

    private static void CheckParcelDetails(UpdateRealEstateValuationDetailRequest request)
    {
        if (request.SpecificDetails is not ParcelDetails)
            throw new CisAuthorizationException("SpecificDetails does not contain the parcel object");

        if (request.RealEstateStateId.HasValue)
            throw new CisAuthorizationException("RealEstateStateId has to be null for the parcel variant");
    }

    private static void CheckOtherDetails(UpdateRealEstateValuationDetailRequest request)
    {
        if (request.RealEstateStateId.HasValue)
            return;

        throw new CisAuthorizationException("RealEstateStateId is required");
    }
}