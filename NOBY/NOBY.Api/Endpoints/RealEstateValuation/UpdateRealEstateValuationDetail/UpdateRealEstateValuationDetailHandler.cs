using System.Text.Json;
using SharedTypes.Enums;
using DomainServices.CaseService.Clients;
using DomainServices.RealEstateValuationService.Clients;
using NOBY.Dto.RealEstateValuation;
using NOBY.Dto.RealEstateValuation.SpecificDetails;
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
        var valuationDetail = await _realEstateValuationService.GetRealEstateValuationDetail(request.RealEstateValuationId, cancellationToken);

        await CheckIfRequestIsValid(request, valuationDetail, cancellationToken);

        if (valuationDetail.ValuationStateId is not 7)
            await _realEstateValuationService.UpdateStateByRealEstateValuation(request.RealEstateValuationId, 7, cancellationToken);

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
                dsRequest.ParcelDetails = new __Contracts.SpecificDetailParcelObject();
                if (parcelDetails?.ParcelNumbers is not null)
                {
                    dsRequest.ParcelDetails.ParcelNumbers.AddRange(parcelDetails.ParcelNumbers.Select(t => new __Contracts.SpecificDetailParcelNumber
                    {
                        Number = t.Number,
                        Prefix = t.Prefix
                    }));
                }
                break;
        }

        await _realEstateValuationService.UpdateRealEstateValuationDetail(dsRequest, cancellationToken);
    }

    private async Task CheckIfRequestIsValid(UpdateRealEstateValuationDetailRequest request, __Contracts.RealEstateValuationDetail valuationDetail, CancellationToken cancellationToken)
    {
        var caseInstance = await _caseService.GetCaseDetail(request.CaseId, cancellationToken);

        if (valuationDetail.CaseId != request.CaseId)
            throw new NobyValidationException(90032, "The requested RealEstateValuation is not assigned to the requested Case");

        if (valuationDetail.ValuationStateId is not (6 or 7))
            throw new NobyValidationException(90032, "The valuation has bad state");

        if (caseInstance.State == (int)CaseStates.InProgress && request.LoanPurposeDetails is not null)
        {
            throw new NobyValidationException(90032, "The LoanPurposeDetails has to be null when the case is in progress");
        }

        if (caseInstance.State == (int)CaseStates.InProgress && request.IsLoanRealEstate != valuationDetail.IsLoanRealEstate)
        {
            throw new NobyValidationException(90032, "request.IsLoanRealEstate != valuationDetail.IsLoanRealEstate");
        }

        if (valuationDetail.PossibleValuationTypeId is not null && valuationDetail.PossibleValuationTypeId.Count > 0)
        {
            throw new NobyValidationException(90032, "PossibleValuationTypeId is not empty");
        }

        var variant = RealEstateVariantHelper.GetRealEstateVariant(valuationDetail.RealEstateTypeId);

        ParseAndSetSpecificDetails(request, variant);

        switch (variant)
        {
            case RealEstateVariants.HouseAndFlat: CheckHouseAndFlatDetails(request);
                break;

            case RealEstateVariants.OnlyFlat: CheckOnlyFlatDetails(request);
                break;

            case RealEstateVariants.Parcel: CheckParcelDetails(request);
                break;

            case RealEstateVariants.Other: CheckOtherDetails(request);
                break;

            default: throw new NotImplementedException();
        }
    }

    private static void ParseAndSetSpecificDetails(UpdateRealEstateValuationDetailRequest request, RealEstateVariants variant)
    {
        if (request.SpecificDetails is not JsonElement jsonElement)
            return;

        var jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        request.SpecificDetails = variant switch
        {
            RealEstateVariants.HouseAndFlat or RealEstateVariants.OnlyFlat => jsonElement.Deserialize<HouseAndFlatDetails>(jsonOptions),
            RealEstateVariants.Parcel => jsonElement.Deserialize<ParcelDetails>(jsonOptions),
            _ => default
        };
    }

    private static void CheckHouseAndFlatDetails(UpdateRealEstateValuationDetailRequest request)
    {
        if (request.SpecificDetails is not HouseAndFlatDetails houseAndFlatDetails)
            throw new NobyValidationException("SpecificDetails does not contain the HouseAndFlatDetails object");

        if (houseAndFlatDetails.FinishedHouseAndFlatDetails is null)
        {
            if (request.RealEstateStateId == (int)RealEstateStateId.Finished)
                throw new NobyValidationException("FinishedHouseAndFlatDetails object is null and request RealEstateStateId is Finished");
        }
        else if (request.RealEstateStateId != (int)RealEstateStateId.Finished)
        {
            throw new NobyValidationException("FinishedHouseAndFlatDetails is not null and RealEstateStateId is not Finished");
        }
    }

    private static void CheckOnlyFlatDetails(UpdateRealEstateValuationDetailRequest request)
    {
        CheckHouseAndFlatDetails(request);

        if ((request.SpecificDetails as HouseAndFlatDetails)?.FlatOnlyDetails is null)
            throw new NobyValidationException("The FlatOnlyDetails is required");
    }

    private static void CheckParcelDetails(UpdateRealEstateValuationDetailRequest request)
    {
        if (request.SpecificDetails is not ParcelDetails)
            throw new NobyValidationException("SpecificDetails does not contain the parcel object");

        if (request.RealEstateStateId.HasValue)
            throw new NobyValidationException("RealEstateStateId has to be null for the parcel variant");
    }

    private static void CheckOtherDetails(UpdateRealEstateValuationDetailRequest request)
    {
        if (request.RealEstateStateId.HasValue)
            return;

        throw new NobyValidationException("RealEstateStateId is required");
    }
}