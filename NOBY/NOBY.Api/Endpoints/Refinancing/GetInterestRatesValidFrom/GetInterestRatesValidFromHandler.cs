using NOBY.Services.InterestRatesValidFrom;

namespace NOBY.Api.Endpoints.Refinancing.GetInterestRatesValidFrom;

internal sealed class GetInterestRatesValidFromHandler
    : IRequestHandler<GetInterestRatesValidFromRequest, GetInterestRatesValidFromResponse>
{
    public async Task<GetInterestRatesValidFromResponse> Handle(GetInterestRatesValidFromRequest request, CancellationToken cancellationToken)
    {
        var result = await _ratesValidFromService.GetValidityDates(request.CaseId, cancellationToken);

        return new GetInterestRatesValidFromResponse
        {
            InterestRatesValidFrom = 
            [
                new GetInterestRatesValidFromResponseItem { InterestRateValidFrom = result.Date1 },
                new GetInterestRatesValidFromResponseItem { InterestRateValidFrom = result.Date2, IsDefault = true }
            ]
        };
    }

    private readonly InterestRatesValidFromService _ratesValidFromService;

    public GetInterestRatesValidFromHandler(InterestRatesValidFromService ratesValidFromService)
    {
        _ratesValidFromService = ratesValidFromService;
    }
}
