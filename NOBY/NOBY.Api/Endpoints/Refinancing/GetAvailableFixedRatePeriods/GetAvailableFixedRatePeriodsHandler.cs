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
        var offers = await _offerService.GetOfferList(request.CaseId, DomainServices.OfferService.Contracts.OfferTypes.MortgageRefixation, false, cancellationToken);

        // periody pouzite v ulozenych offers
        List<int> usedPeriods = offers
            ?.Where(t =>
            {
                var flag = (OfferFlagTypes)t.Data.Flags;
                return flag.HasFlag(OfferFlagTypes.Current) && !flag.HasFlag(OfferFlagTypes.Communicated);
            })
            .Select(t => t.MortgageRefixation.SimulationInputs.FixedRatePeriod)
            .ToList() ?? [];

        // mozne periody pro dany produkt
        var availablePeriods = (await _codebookService.FixedRatePeriods(cancellationToken))
            .Where(t => t.IsValid && t.MandantId == (int)Mandants.Kb && t.ProductTypeId == productInstance.Mortgage.ProductTypeId)
            .ToList();

        // nevim kdy muze byt FixedRateValidTo NULL, ale na DEVu se mi to stavalo... co s tim?
        DateTime productFixedRateValidTo = (DateTime?)productInstance.Mortgage.FixedRateValidTo ?? DateTime.Now;

        // prvni perioda fixace, ktera presahuje splatnost uveru
        int? minOverflowedPeriod = availablePeriods
            .Where(t => productFixedRateValidTo.AddMonths(t.FixedRatePeriod) > productInstance.Mortgage.LoanDueDate)
            .Select(t => t.FixedRatePeriod)
            .Order()
            .FirstOrDefault();

        var finalPeriods = availablePeriods
            .Where(t =>
            {
                if (usedPeriods.Contains(t.FixedRatePeriod))
                {
                    return false;
                }

                if (productFixedRateValidTo.AddMonths(t.FixedRatePeriod) > productInstance.Mortgage.LoanDueDate && t.FixedRatePeriod != minOverflowedPeriod)
                {
                    return false;
                }

                return true;
            })
            .Select(t => t.FixedRatePeriod)
            .Order()
            .ToList();

        return new GetAvailableFixedRatePeriodsResponse
        {
            AvailableFixedRatePeriods = finalPeriods
        };
    }
}
