using DomainServices.CodebookService.Clients;
using DomainServices.OfferService.Clients.v1;
using DomainServices.ProductService.Clients;

namespace NOBY.Api.Endpoints.Offer.SimulateMortgageRefixation;

internal sealed class SimulateMortgageRefixationHandler(
    IOfferServiceClient _offerService, 
    ICodebookServiceClient _codebookService, 
    IProductServiceClient _productService)
        : IRequestHandler<SimulateMortgageRefixationRequest, SimulateMortgageRefixationResponse>
{
    public async Task<SimulateMortgageRefixationResponse> Handle(SimulateMortgageRefixationRequest request, CancellationToken cancellationToken)
    {
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

        // validace rate
        if (request.InterestRateDiscount.HasValue && (interestRate - request.InterestRateDiscount.Value) < 0.1M)
        {
            throw new NobyValidationException(90060);
        }

        var dsRequest = new DomainServices.OfferService.Contracts.SimulateMortgageRefixationRequest
        {
            CaseId = request.CaseId,
            ValidTo = product.Mortgage.FixedRateValidTo,
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
            InterestRate = interestRate,
            InterestRateDiscount = request.InterestRateDiscount,
            LoanPaymentAmount = result.SimulationResults.LoanPaymentAmount,
            LoanPaymentAmountDiscounted = result.SimulationResults.LoanPaymentAmountDiscounted
        };
    }
}
