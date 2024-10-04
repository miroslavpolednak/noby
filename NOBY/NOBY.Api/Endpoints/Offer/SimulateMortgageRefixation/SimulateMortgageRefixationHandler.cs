using DomainServices.CodebookService.Clients;
using DomainServices.OfferService.Clients.v1;
using DomainServices.ProductService.Clients;
using NOBY.Services.MortgageRefinancing;

namespace NOBY.Api.Endpoints.Offer.SimulateMortgageRefixation;

internal sealed class SimulateMortgageRefixationHandler(
    IOfferServiceClient _offerService,
    MortgageRefinancingDataService _refinancingDataService,
    ICodebookServiceClient _codebookService,
    IProductServiceClient _productService)
        : IRequestHandler<OfferSimulateMortgageRefixationRequest, List<OfferSimulateMortgageRefixationResponse>>
{
    public async Task<List<OfferSimulateMortgageRefixationResponse>> Handle(OfferSimulateMortgageRefixationRequest request, CancellationToken cancellationToken)
    {
        // validace zda na Case jiz neexistuje simulace se stejnou delkou fixace
        var existingOffers = await _offerService.GetOfferList(request.CaseId, DomainServices.OfferService.Contracts.OfferTypes.MortgageRefixation, false, cancellationToken: cancellationToken);
        if (existingOffers.Any(t => ((EnumOfferFlagTypes)t.Data.Flags).HasFlag(EnumOfferFlagTypes.Current) && request.FixedRatePeriods.Any(x => x == t.MortgageRefixation.SimulationInputs.FixedRatePeriod)))
        {
            throw new NobyValidationException("Offer with the same fixed period already exist");
        }

        // info o hypotece kvuli FixedRateValidTo
        var product = await _productService.GetMortgage(request.CaseId, cancellationToken);

        // validace fixed period
        var periods = await _codebookService.FixedRatePeriods(cancellationToken);
        if (!request.FixedRatePeriods.All(t => periods.Any(x => x.IsValid && x.ProductTypeId == product.Mortgage.ProductTypeId && x.FixedRatePeriod == t && !x.IsNewProduct)))
        {
            throw new NobyValidationException("FixedRatePeriod cant be validated");
        }

        var validFrom = ((DateOnly?)product.Mortgage.FixedRateValidTo ?? DateOnly.MinValue).AddDays(1);

        // aktivni IC
        decimal? currentInterestRateDiscount = request.ProcessId.HasValue ? (await _refinancingDataService.GetActivePriceException(request.CaseId, request.ProcessId.Value, cancellationToken))?.LoanInterestRate?.LoanInterestRateDiscount : null;

        var interestRates = await GetInterestRates(request.CaseId, validFrom, request.FixedRatePeriods, currentInterestRateDiscount, cancellationToken);

        // vytvorit vsechny simulace
        List<OfferSimulateMortgageRefixationResponse> response = [];

        foreach (var period in request.FixedRatePeriods)
        {
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
                    InterestRate = interestRates[period],
                    InterestRateDiscount = currentInterestRateDiscount,
                    FixedRatePeriod = period,
                    InterestRateValidFrom = validFrom
                }
            };

            // spocitat simulaci
            var result = await _offerService.SimulateMortgageRefixation(dsRequest, cancellationToken);

            response.Add(new OfferSimulateMortgageRefixationResponse
            {
                OfferId = result.OfferId,
                InterestRate = interestRates[period],
                InterestRateDiscount = currentInterestRateDiscount,
                LoanPaymentAmount = result.SimulationResults.LoanPaymentAmount,
                LoanPaymentAmountDiscounted = result.SimulationResults.LoanPaymentAmountDiscounted
            });
        }

        return response;
    }

    private async Task<Dictionary<int, decimal>> GetInterestRates(long caseId, DateOnly validFrom, List<int> fixedRatePeriods, decimal? currentInterestRateDiscount, CancellationToken cancellationToken)
    {
        var interestRates = new Dictionary<int, decimal>(fixedRatePeriods.Count);

        foreach (var fixedRatePeriod in fixedRatePeriods)
        {
            // ziskat int.rate
            var interestRate = await _offerService.GetInterestRate(caseId, validFrom, fixedRatePeriod, cancellationToken);

            // validace rate
            if (currentInterestRateDiscount.GetValueOrDefault() > 0 && (interestRate - currentInterestRateDiscount!.Value) < 0.1M)
            {
                throw new NobyValidationException(90060);
            }

            interestRates[fixedRatePeriod] = interestRate;
        }

        return interestRates;
    }
}
