using CIS.Foms.Enums;
using DomainServices.CaseService.Clients;
using DomainServices.OfferService.Clients;
using DomainServices.ProductService.Clients;
using DomainServices.RealEstateValuationService.Clients;
using DomainServices.SalesArrangementService.Clients;

namespace NOBY.Api.Endpoints.RealEstateValuation.GetRealEstateValuationTypes;

internal sealed class GetRealEstateValuationTypesHandler
    : IRequestHandler<GetRealEstateValuationTypesRequest, List<int>>
{
    public async Task<List<int>> Handle(GetRealEstateValuationTypesRequest request, CancellationToken cancellationToken)
    {
        var dsRequest = new DomainServices.RealEstateValuationService.Contracts.GetRealEstateValuationTypesRequest
        {
            DealType = "HYPO",
            RealEstateValuationId = request.RealEstateValuationId
        };

        var caseInstance = await _caseService.ValidateCaseId(request.CaseId, false, cancellationToken);
        if (caseInstance.State!.Value == (int)CaseStates.InProgress)
        {
            var productSA = await _salesArrangementService.GetProductSalesArrangement(request.CaseId, cancellationToken);
            var offerInstance = await _offerService.GetMortgageOfferDetail(productSA.OfferId!.Value, cancellationToken);

            dsRequest.LoanAmount = offerInstance.SimulationResults.LoanAmount;
            if (offerInstance.SimulationInputs.LoanPurposes?.Any() ?? false)
            {
                dsRequest.LoanPurposes.AddRange(offerInstance.SimulationInputs.LoanPurposes.Select(t => t.LoanPurposeId));
            }
        }
        else
        {
            var product = await _productService.GetMortgage(request.CaseId, cancellationToken);
            if (product.Mortgage is null)
            {
                throw new NobyValidationException("Product.Mortgage object is null");
            }
            var detail = await _realEstateValuationService.GetRealEstateValuationDetail(request.RealEstateValuationId, cancellationToken);

            dsRequest.LoanAmount = product.Mortgage.CurrentAmount;
            if (detail.LoanPurposeDetails?.LoanPurposes?.Any() ?? false)
            {
                dsRequest.LoanPurposes.AddRange(detail.LoanPurposeDetails.LoanPurposes);
            }
        }

        var result = await _realEstateValuationService.GetRealEstateValuationTypes(dsRequest, cancellationToken);

        return result.Select(t => (int)t).ToList();
    }

    private readonly IProductServiceClient _productService;
    private readonly IOfferServiceClient _offerService;
    private readonly ISalesArrangementServiceClient _salesArrangementService;
    private readonly ICaseServiceClient _caseService;
    private readonly IRealEstateValuationServiceClient _realEstateValuationService;

    public GetRealEstateValuationTypesHandler(
        IOfferServiceClient offerService,
        IProductServiceClient productService,
        IRealEstateValuationServiceClient realEstateValuationService, 
        ICaseServiceClient caseService, 
        ISalesArrangementServiceClient salesArrangementService)
    {
        _productService = productService;
        _offerService = offerService;
        _salesArrangementService = salesArrangementService;
        _caseService = caseService;
        _realEstateValuationService = realEstateValuationService;
    }
}

