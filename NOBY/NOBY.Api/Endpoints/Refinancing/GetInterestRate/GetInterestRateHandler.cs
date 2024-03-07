using DomainServices.OfferService.Clients;
using NOBY.Services.InterestRatesValidFrom;

namespace NOBY.Api.Endpoints.Refinancing.GetInterestRate;

internal sealed class GetInterestRateHandler
    : IRequestHandler<GetInterestRateRequest, GetInterestRateResponse>
{
    public async Task<GetInterestRateResponse> Handle(GetInterestRateRequest request, CancellationToken cancellationToken)
    {
        // zjistit datumy, pouzit vychozi
        var validityDates = await _ratesValidFromService.GetValidityDates(request.CaseId, cancellationToken);

        var result = await _offerService.GetInterestRate(request.CaseId, validityDates.Date2, cancellationToken);
        return new GetInterestRateResponse
        {
            LoanInterestRateCurrent = result
        };
    }

    private readonly IOfferServiceClient _offerService;
    private readonly InterestRatesValidFromService _ratesValidFromService;

    public GetInterestRateHandler(IOfferServiceClient offerService, InterestRatesValidFromService ratesValidFromService)
    {
        _ratesValidFromService = ratesValidFromService;
        _offerService = offerService;
    }
}
