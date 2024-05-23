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
        : IRequestHandler<SimulateMortgageRefixationRequest, List<Dto.Refinancing.RefinancingSimulationResult>>
{
    public async Task<List<Dto.Refinancing.RefinancingSimulationResult>> Handle(SimulateMortgageRefixationRequest request, CancellationToken cancellationToken)
    {
        // validace zda na Case jiz neexistuje simulace se stejnou delkou fixace
        var existingOffers = await _offerService.GetOfferList(request.CaseId, DomainServices.OfferService.Contracts.OfferTypes.MortgageRefixation, false, cancellationToken);
        if (existingOffers.Any(t => ((OfferFlagTypes)t.Data.Flags).HasFlag(OfferFlagTypes.Current) && request.FixedRatePeriods.Any(x => x == t.MortgageRefixation.SimulationInputs.FixedRatePeriod)))
        {
            throw new NobyValidationException("Offer with the same fixed period already exist");
        }

        // info o hypotece kvuli FixedRateValidTo
        var product = await _productService.GetMortgage(request.CaseId, cancellationToken);

        // validace fixed period
        var periods = await _codebookService.FixedRatePeriods(cancellationToken);
        if (!request.FixedRatePeriods.All(t => periods.Any(x => x.IsValid && x.ProductTypeId == product.Mortgage.ProductTypeId && x.FixedRatePeriod == t))) // && !x.IsNewProduct
		{
            throw new NobyValidationException("FixedRatePeriod cant be validated");
        }

        var validFrom = ((DateTime?)product.Mortgage.FixedRateValidTo ?? DateTime.MinValue).AddDays(1);

        // ziskat int.rate
        var interestRate = await _offerService.GetInterestRate(request.CaseId, validFrom, cancellationToken);

        // aktivni IC
        decimal? currentInterestRateDiscount = request.ProcessId.HasValue ? (await _refinancingDataService.GetActivePriceException(request.CaseId, request.ProcessId.Value, cancellationToken))?.LoanInterestRate?.LoanInterestRateDiscount : null;

        // validace rate
        if (currentInterestRateDiscount.GetValueOrDefault() > 0 && (interestRate - currentInterestRateDiscount!.Value) < 0.1M)
        {
            throw new NobyValidationException(90060);
        }

        // vytvorit vsechny simulace
        List<Dto.Refinancing.RefinancingSimulationResult> response = [];

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
                    InterestRate = interestRate,
                    InterestRateDiscount = currentInterestRateDiscount,
                    FixedRatePeriod = period,
                    InterestRateValidFrom = validFrom
                }
            };

            // spocitat simulaci
            var result = await _offerService.SimulateMortgageRefixation(dsRequest, cancellationToken);

            response.Add(new Dto.Refinancing.RefinancingSimulationResult
            {
                OfferId = result.OfferId,
                InterestRate = interestRate,
                InterestRateDiscount = currentInterestRateDiscount,
                LoanPaymentAmount = result.SimulationResults.LoanPaymentAmount,
                LoanPaymentAmountDiscounted = result.SimulationResults.LoanPaymentAmountDiscounted
            });
        }

        return response;
    }
}
