using DomainServices.CaseService.Clients.v1;
using DomainServices.OfferService.Clients;
using DomainServices.ProductService.Clients;
using DomainServices.RealEstateValuationService.Clients;
using DomainServices.SalesArrangementService.Clients;
using NOBY.Infrastructure.ErrorHandling;

namespace NOBY.Services.RealEstateValuationType;

[TransientService, AsImplementedInterfacesService]
internal sealed class RealEstateValuationTypeService
    : IRealEstateValuationTypeService
{
    public async Task<List<RealEstateValuationTypes>> GetAllowedTypes(int realEstateValuationId, long caseId, CancellationToken cancellationToken)
    {
        var dsRequest = new DomainServices.RealEstateValuationService.Contracts.GetRealEstateValuationTypesRequest
        {
            DealType = "HYPO",
            RealEstateValuationId = realEstateValuationId
        };

        var caseInstance = await _caseService.ValidateCaseId(caseId, false, cancellationToken);
        var detail = await _realEstateValuationService.GetRealEstateValuationDetail(realEstateValuationId, cancellationToken);

        if (detail.PossibleValuationTypeId?.Any() ?? false)
        {
            return detail.PossibleValuationTypeId.Select(t => (RealEstateValuationTypes)t).ToList();
        }
        else
        {
            if (caseInstance.State!.Value == (int)CaseStates.InProgress)
            {
                var productSA = (await _salesArrangementService.GetProductSalesArrangements(caseId, cancellationToken)).First();
                var offerInstance = await _offerService.GetOfferDetail(productSA.OfferId!.Value, cancellationToken);

                dsRequest.LoanAmount = offerInstance.MortgageOffer.SimulationResults.LoanAmount;
                if (offerInstance.MortgageOffer.SimulationInputs.LoanPurposes?.Any() ?? false)
                {
                    dsRequest.LoanPurposes.AddRange(offerInstance.MortgageOffer.SimulationInputs.LoanPurposes.Select(t => t.LoanPurposeId));
                }
            }
            else
            {
                var product = await _productService.GetMortgage(caseId, cancellationToken);
                if (product.Mortgage is null)
                {
                    throw new NobyValidationException("Product.Mortgage object is null");
                }

                dsRequest.LoanAmount = product.Mortgage.CurrentAmount;
                if (detail.LoanPurposeDetails?.LoanPurposes?.Any() ?? false)
                {
                    dsRequest.LoanPurposes.AddRange(detail.LoanPurposeDetails.LoanPurposes);
                }
            }

            var result = await _realEstateValuationService.GetRealEstateValuationTypes(dsRequest, cancellationToken);

            return result.Select(t => t).ToList();
        }
    }

    private readonly IProductServiceClient _productService;
    private readonly IOfferServiceClient _offerService;
    private readonly ISalesArrangementServiceClient _salesArrangementService;
    private readonly ICaseServiceClient _caseService;
    private readonly IRealEstateValuationServiceClient _realEstateValuationService;

    public RealEstateValuationTypeService(
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
