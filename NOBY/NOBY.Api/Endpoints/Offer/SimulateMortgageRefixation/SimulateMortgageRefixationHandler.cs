using DomainServices.CaseService.Clients.v1;
using DomainServices.CodebookService.Clients;
using DomainServices.OfferService.Clients.v1;
using DomainServices.ProductService.Clients;

namespace NOBY.Api.Endpoints.Offer.SimulateMortgageRefixation;

internal sealed class SimulateMortgageRefixationHandler
    : IRequestHandler<SimulateMortgageRefixationRequest, SimulateMortgageRefixationResponse>
{
    public async Task<SimulateMortgageRefixationResponse> Handle(SimulateMortgageRefixationRequest request, CancellationToken cancellationToken)
    {
        // validace na stav case
        var caseInstance = await _caseService.ValidateCaseId(request.CaseId, true, cancellationToken);
        if (caseInstance.State!.Value is not ((int)CaseStates.InAdministration or (int)CaseStates.InDisbursement))
        {
            throw new NobyValidationException("CaseState is not 4,5");
        }

        // validace zda na Case jiz neexistuje simulace se stejnou delkou fixace
        var existingOffers = await _offerService.GetOfferList(request.CaseId, DomainServices.OfferService.Contracts.OfferTypes.MortgageRefixation, false, cancellationToken);
        if (existingOffers.Any(t => ((OfferFlagTypes)t.Data.Flags).HasFlag(OfferFlagTypes.Current) && t.MortgageRefixation.SimulationInputs.FixedRatePeriod == request.FixedRatePeriod))
        {
            throw new NobyValidationException("Offer with the same fixed period already exist");
        }

        // info o hypotece kvuli FixedRateValidTo
        var product = await _productService.GetMortgage(request.CaseId, cancellationToken);

        // validace fixed period
        var periods = await _codebookService.FixedRatePeriods(cancellationToken);
        if (!periods.Any(t => t.IsValid && t.FixedRatePeriod == request.FixedRatePeriod && t.ProductTypeId == product.Mortgage.ProductTypeId))
        {
            throw new NobyValidationException("FixedRatePeriod cant be validated");
        }

        var validFrom = ((DateTime?)product.Mortgage.FixedRateValidTo ?? DateTime.MinValue).AddDays(1);

        // ziskat int.rate
        var interestRate = await _offerService.GetInterestRate(request.CaseId, validFrom, cancellationToken);

        var dsRequest = new DomainServices.OfferService.Contracts.SimulateMortgageRefixationRequest
        {
            CaseId = request.CaseId,
            BasicParameters = new()
            {
                FixedRateValidTo = (DateTime)product.Mortgage.FixedRateValidTo!
            },
            SimulationInputs = new()
            {
                InterestRate = interestRate,
                InterestRateDiscount = request.InterestRateDiscount,
                FixedRatePeriod = request.FixedRatePeriod,
                InterestRateValidFrom = validFrom
            }
        };
        
        // spocitat simulaci
        var result = await _offerService.SimulateMortgageRefixation(dsRequest, cancellationToken);

        return new SimulateMortgageRefixationResponse
        {
            OfferId = result.OfferId,
            LoanPaymentAmount = result.SimulationResults.LoanPaymentAmount,
            LoanPaymentAmountIndividualPrice = result.SimulationResults.LoanPaymentAmountDiscounted
        };
    }

    private readonly IProductServiceClient _productService;
    private readonly ICodebookServiceClient _codebookService;
    private readonly IOfferServiceClient _offerService;
    private readonly ICaseServiceClient _caseService;

    public SimulateMortgageRefixationHandler(IOfferServiceClient offerService, ICodebookServiceClient codebookService, IProductServiceClient productService, ICaseServiceClient caseService)
    {
        _offerService = offerService;
        _codebookService = codebookService;
        _productService = productService;
        _caseService = caseService;
    }
}
