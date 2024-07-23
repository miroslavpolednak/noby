using DomainServices.OfferService.Clients.v1;
using NOBY.Services.InterestRatesValidFrom;

namespace NOBY.Api.Endpoints.Refinancing.GetInterestRate;

internal sealed class GetInterestRateHandler(
    IOfferServiceClient _offerService, 
    InterestRatesValidFromService _ratesValidFromService)
        : IRequestHandler<GetInterestRateRequest, RefinancingGetInterestRateResponse>
{
    public async Task<RefinancingGetInterestRateResponse> Handle(GetInterestRateRequest request, CancellationToken cancellationToken)
    {
        // zjistit datumy, pouzit vychozi
        var (_, date2) = await _ratesValidFromService.GetValidityDates(request.CaseId, cancellationToken);

        var result = await _offerService.GetInterestRate(request.CaseId, date2, cancellationToken: cancellationToken);
        return new()
        {
            LoanInterestRateCurrent = result
        };
    }
}
