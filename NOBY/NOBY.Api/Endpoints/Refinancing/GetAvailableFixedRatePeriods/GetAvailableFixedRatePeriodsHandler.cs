using DomainServices.CodebookService.Clients;
using DomainServices.OfferService.Clients.v1;
using DomainServices.ProductService.Clients;

namespace NOBY.Api.Endpoints.Refinancing.GetAvailableFixedRatePeriods;

internal sealed class GetAvailableFixedRatePeriodsHandler(
    IProductServiceClient _productService,
    IOfferServiceClient _offerService,
    ICodebookServiceClient _codebookService)
        : IRequestHandler<GetAvailableFixedRatePeriodsRequest, GetAvailableFixedRatePeriodsResponse>
{
    public async Task<GetAvailableFixedRatePeriodsResponse> Handle(GetAvailableFixedRatePeriodsRequest request, CancellationToken cancellationToken)
    {
        // info o hypo
        var productInstance = await _productService.GetMortgage(request.CaseId, cancellationToken);
        
        // seznam jiz ulozenych offer
        var offers = await _offerService.GetOfferList(request.CaseId, DomainServices.OfferService.Contracts.OfferTypes.MortgageRefixation, true, cancellationToken);
        // periody pouzite v ulozenych offers
        var usedPeriods = offers
            .Where(t =>
            {
                var flag = (OfferFlagTypes)t.Data.Flags;
                return flag.HasFlag(OfferFlagTypes.Current) && !flag.HasFlag(OfferFlagTypes.Communicated);
            })
            .Select(t => t.MortgageRefixation.SimulationInputs.FixedRatePeriod)
            .ToList();

        // mozne periody pro dany produkt
        var availablePeriods = (await _codebookService.FixedRatePeriods(cancellationToken))
            .Where(t => t.IsValid && t.MandantId == (int)Mandants.Kb && t.ProductTypeId == productInstance.Mortgage.ProductTypeId)
            .ToList();

        var xxx = availablePeriods
            .Where(t => !usedPeriods.Contains(t.FixedRatePeriod))
            .Select(t => new { t.FixedRatePeriod, DateTo = ((DateTime)productInstance.Mortgage.LoanInterestRateValidToRefinancing).AddMonths(t.FixedRatePeriod) })
            .OrderBy(t => t.FixedRatePeriod);
        
        throw new NotImplementedException();
    }
}
