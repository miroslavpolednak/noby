using DomainServices.CaseService.Clients.v1;
using DomainServices.OfferService.Clients.v1;
using DomainServices.ProductService.Clients;
using DomainServices.RealEstateValuationService.Clients;
using DomainServices.SalesArrangementService.Clients;
using NOBY.Dto.RealEstateValuation;
using RealEstateValuationDetail = DomainServices.RealEstateValuationService.Contracts.RealEstateValuationDetail;

namespace NOBY.Api.Endpoints.RealEstateValuation.GetRealEstateValuationTypes;

internal sealed class GetRealEstateValuationTypesHandler
    : IRequestHandler<GetRealEstateValuationTypesRequest, List<int>>
{
    private readonly ICaseServiceClient _caseService;
    private readonly ISalesArrangementServiceClient _salesArrangementService;
    private readonly IOfferServiceClient _offerService;
    private readonly IProductServiceClient _productService;
    private readonly IRealEstateValuationServiceClient _realEstateValuationService;

    public GetRealEstateValuationTypesHandler(ICaseServiceClient caseService,
                                              ISalesArrangementServiceClient salesArrangementService,
                                              IOfferServiceClient offerService,
                                              IProductServiceClient productService,
                                              IRealEstateValuationServiceClient realEstateValuationService)
    {
        _caseService = caseService;
        _salesArrangementService = salesArrangementService;
        _offerService = offerService;
        _productService = productService;
        _realEstateValuationService = realEstateValuationService;
    }

    public async Task<List<int>> Handle(GetRealEstateValuationTypesRequest request, CancellationToken cancellationToken)
    {
        var detail = await _realEstateValuationService.GetRealEstateValuationDetail(request.RealEstateValuationId, cancellationToken);
        var caseInstance = await _caseService.ValidateCaseId(request.CaseId, throwExceptionIfNotFound: true, cancellationToken);

        if (detail.CaseId != request.CaseId)
            throw new NobyValidationException(90032, "RealEstateValuationDetail is for different case");

        if (detail.ValuationStateId != (int)RealEstateValuationStates.Rozpracovano)
            throw new NobyValidationException(90032, "RealEstateValuation is not in progress state (7)");

        if (detail.PossibleValuationTypeId.Any())
            return [.. detail.PossibleValuationTypeId];

        ValidateRealEstateValuationDetail(detail);

        var dsRequest = new DomainServices.RealEstateValuationService.Contracts.GetRealEstateValuationTypesRequest
        {
            DealType = "HYPO",
            RealEstateValuationId = request.RealEstateValuationId
        };

        if (caseInstance.State!.Value == (int)CaseStates.InProgress)
        {
            var productSA = (await _salesArrangementService.GetProductSalesArrangements(request.CaseId, cancellationToken)).First();
            var offerInstance = await _offerService.GetOffer(productSA.OfferId!.Value, cancellationToken);

            if (!offerInstance.MortgageOffer.SimulationInputs.LoanPurposes.Any())
                throw new NobyValidationException(90032, "LoanPurposes collection is empty");

            dsRequest.LoanAmount = offerInstance.MortgageOffer.SimulationResults.LoanAmount; 
            dsRequest.LoanPurposes.AddRange(offerInstance.MortgageOffer.SimulationInputs.LoanPurposes.Select(t => t.LoanPurposeId));
        }
        else
        {
            var product = await _productService.GetMortgage(request.CaseId, cancellationToken);

            dsRequest.LoanAmount = ((decimal?)product.Mortgage.AvailableForDrawing).GetValueOrDefault() > 0 ? product.Mortgage.LoanAmount : product.Mortgage.CurrentAmount;
            dsRequest.LoanPurposes.AddRange(detail.LoanPurposeDetails.LoanPurposes);
        }

        var result = await _realEstateValuationService.GetRealEstateValuationTypes(dsRequest, cancellationToken);

        return result.Cast<int>().ToList();
    }

    private static void ValidateRealEstateValuationDetail(RealEstateValuationDetail detail)
    {
        if (!detail.RealEstateSubtypeId.HasValue)
            throw new NobyValidationException(90032);

        var variant = RealEstateVariantHelper.GetRealEstateVariant(detail.RealEstateTypeId);

        switch (variant)
        {
            case RealEstateVariants.HouseAndFlat: CheckHouseAndFlatDetails(detail);
                break;

            case RealEstateVariants.OnlyFlat: CheckHouseAndFlatDetails(detail);
                break;

            case RealEstateVariants.Parcel: CheckParcelDetails(detail);
                break;

            case RealEstateVariants.Other: CheckOtherDetails(detail);
                break;

            default: throw new NotImplementedException();
        }
    }

    private static void CheckHouseAndFlatDetails(RealEstateValuationDetail detail)
    {
        if (detail.SpecificDetailCase != RealEstateValuationDetail.SpecificDetailOneofCase.HouseAndFlatDetails)
            throw new NobyValidationException(90032, "SpecificDetails does not contain the HouseAndFlat object");

        if (detail.HouseAndFlatDetails.FinishedHouseAndFlatDetails is null)
        {
            if (detail.RealEstateStateId == (int)RealEstateStateId.Finished)
                throw new NobyValidationException("FinishedHouseAndFlatDetails object is null and request RealEstateStateId is Finished");
        } 
        else if (detail.RealEstateStateId != (int)RealEstateStateId.Finished)
        {
            throw new NobyValidationException("FinishedHouseAndFlatDetails is not null and RealEstateStateId is not Finished");
        }
    }

    private static void CheckParcelDetails(RealEstateValuationDetail detail)
    {
        if (detail.SpecificDetailCase != RealEstateValuationDetail.SpecificDetailOneofCase.ParcelDetails)
            throw new NobyValidationException(90032, "SpecificDetails does not contain the parcel object");

        if (detail.RealEstateStateId.HasValue)
            throw new NobyValidationException(90032, "RealEstateStateId has to be null for the parcel variant");

        if (!detail.ParcelDetails.ParcelNumbers.Any())
            throw new NobyValidationException(90032, "ParcelNumbers collection is empty");
    }

    private static void CheckOtherDetails(RealEstateValuationDetail detail)
    {
        if (detail.SpecificDetailCase != RealEstateValuationDetail.SpecificDetailOneofCase.None)
            throw new NobyValidationException(90032, "SpecificDetails is not null");

        if (!detail.RealEstateStateId.HasValue)
            throw new NobyValidationException(90032, "RealEstateStateId must not be null.");
    }
}

