using DomainServices.CaseService.Clients.v1;
using DomainServices.RealEstateValuationService.Clients;
using __Contracts = DomainServices.RealEstateValuationService.Contracts;

namespace NOBY.Api.Endpoints.RealEstateValuation.UpdateRealEstateValuationDetail;

internal sealed class UpdateRealEstateValuationDetailHandler(
    ICaseServiceClient _caseService, 
    IRealEstateValuationServiceClient _realEstateValuationService)
        : IRequestHandler<RealEstateValuationUpdateRealEstateValuationDetailRequest>
{
    public async Task Handle(RealEstateValuationUpdateRealEstateValuationDetailRequest request, CancellationToken cancellationToken)
    {
        var valuationDetail = await _realEstateValuationService.GetRealEstateValuationDetail(request.RealEstateValuationId, cancellationToken);
        var variant = RealEstateValuationHelpers.GetRealEstateVariant(valuationDetail.RealEstateTypeId);

        await checkIfRequestIsValid(request, variant, valuationDetail, cancellationToken);

        if (valuationDetail.ValuationStateId is not 7)
        {
            await _realEstateValuationService.UpdateStateByRealEstateValuation(request.RealEstateValuationId, WorkflowTaskStates.Rozpracovano, cancellationToken);
        }

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
            },
            LocalSurveyDetails = valuationDetail.LocalSurveyDetails, // pouze zkopírovat
            OnlinePreorderDetails = valuationDetail.OnlinePreorderDetails // pouze zkopírovat
        };

        switch (variant)
        {
            case RealEstateVariants.HouseAndFlat:
            case RealEstateVariants.OnlyFlat:
                dsRequest.HouseAndFlatDetails = new __Contracts.SpecificDetailHouseAndFlatObject
                {
                    PoorCondition = request.SpecificDetails!.HouseAndFlat.PoorCondition,
                    OwnershipRestricted = request.SpecificDetails.HouseAndFlat.OwnershipRestricted,
                    FlatOnlyDetails = request.SpecificDetails.HouseAndFlat.FlatOnlyDetails is null ? null : new __Contracts.SpecificDetailFlatOnlyDetails
                    {
                        SpecialPlacement = request.SpecificDetails.HouseAndFlat.FlatOnlyDetails.SpecialPlacement,
                        Basement = request.SpecificDetails.HouseAndFlat.FlatOnlyDetails.Basement
                    },
                    FinishedHouseAndFlatDetails = request.SpecificDetails.HouseAndFlat.FinishedHouseAndFlatDetails is null ? null : new __Contracts.SpecificDetailFinishedHouseAndFlatDetails
                    {
                        Leased = request.SpecificDetails.HouseAndFlat.FinishedHouseAndFlatDetails.Leased,
                        LeaseApplicable = request.SpecificDetails.HouseAndFlat.FinishedHouseAndFlatDetails.LeaseApplicable

                    }
                };
                break;

            case RealEstateVariants.Parcel:
                dsRequest.ParcelDetails = new __Contracts.SpecificDetailParcelObject();
                if (request.SpecificDetails!.Parcel?.ParcelNumbers is not null)
                {
                    dsRequest.ParcelDetails.ParcelNumbers.AddRange(request.SpecificDetails.Parcel.ParcelNumbers.Select(t => new __Contracts.SpecificDetailParcelNumber
                    {
                        Number = t.Number,
                        Prefix = t.Prefix
                    }));
                }
                break;
        }

        await _realEstateValuationService.UpdateRealEstateValuationDetail(dsRequest, cancellationToken);
    }

    private async Task checkIfRequestIsValid(
        RealEstateValuationUpdateRealEstateValuationDetailRequest request,
        RealEstateVariants variant,
        __Contracts.RealEstateValuationDetail valuationDetail, 
        CancellationToken cancellationToken)
    {
        var caseInstance = await _caseService.GetCaseDetail(request.CaseId, cancellationToken);

        if (valuationDetail.CaseId != request.CaseId)
            throw new NobyValidationException(90032, "The requested RealEstateValuation is not assigned to the requested Case");

        if (valuationDetail.ValuationStateId is not (6 or 7))
            throw new NobyValidationException(90032, "The valuation has bad state");

        if (caseInstance.State == (int)EnumCaseStates.InProgress && request.LoanPurposeDetails is not null)
        {
            throw new NobyValidationException(90032, "The LoanPurposeDetails has to be null when the case is in progress");
        }

        if (caseInstance.State == (int)EnumCaseStates.InProgress && request.IsLoanRealEstate != valuationDetail.IsLoanRealEstate)
        {
            throw new NobyValidationException(90032, "request.IsLoanRealEstate != valuationDetail.IsLoanRealEstate");
        }

        if (valuationDetail.PossibleValuationTypeId is not null && valuationDetail.PossibleValuationTypeId.Count > 0)
        {
            throw new NobyValidationException(90032, "PossibleValuationTypeId is not empty");
        }

        switch (variant)
        {
            case RealEstateVariants.HouseAndFlat: checkHouseAndFlatDetails(request);
                break;

            case RealEstateVariants.OnlyFlat: checkOnlyFlatDetails(request);
                break;

            case RealEstateVariants.Parcel: checkParcelDetails(request);
                break;

            case RealEstateVariants.Other: break;

            default: throw new NotImplementedException();
        }
    }

    private static void checkHouseAndFlatDetails(RealEstateValuationUpdateRealEstateValuationDetailRequest request)
    {
        if (request.SpecificDetails?.HouseAndFlat is null)
        {
            throw new NobyValidationException("SpecificDetails does not contain the HouseAndFlatDetails object");
        }
        else if (request.SpecificDetails.HouseAndFlat.FinishedHouseAndFlatDetails is null)
        {
            if (request.RealEstateStateId == (int)RealEstateStateId.Finished)
                throw new NobyValidationException("FinishedHouseAndFlatDetails object is null and request RealEstateStateId is Finished");
        }
        else if (request.RealEstateStateId != (int)RealEstateStateId.Finished)
        {
            throw new NobyValidationException("FinishedHouseAndFlatDetails is not null and RealEstateStateId is not Finished");
        }
    }

    private static void checkOnlyFlatDetails(RealEstateValuationUpdateRealEstateValuationDetailRequest request)
    {
        checkHouseAndFlatDetails(request);

        if (request.SpecificDetails!.HouseAndFlat.FlatOnlyDetails is null)
            throw new NobyValidationException("The FlatOnlyDetails is required");
    }

    private static void checkParcelDetails(RealEstateValuationUpdateRealEstateValuationDetailRequest request)
    {
        if (request.SpecificDetails?.Parcel is null)
            throw new NobyValidationException("SpecificDetails does not contain the parcel object");

        if (request.RealEstateStateId.HasValue)
            throw new NobyValidationException("RealEstateStateId has to be null for the parcel variant");
    }
}