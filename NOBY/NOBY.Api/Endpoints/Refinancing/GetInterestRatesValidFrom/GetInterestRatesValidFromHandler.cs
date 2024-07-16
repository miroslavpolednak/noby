using NOBY.Services.InterestRatesValidFrom;

namespace NOBY.Api.Endpoints.Refinancing.GetInterestRatesValidFrom;

internal sealed class GetInterestRatesValidFromHandler(InterestRatesValidFromService _ratesValidFromService)
    : IRequestHandler<GetInterestRatesValidFromRequest, GetInterestRatesValidFromResponse>
{
    public async Task<GetInterestRatesValidFromResponse> Handle(GetInterestRatesValidFromRequest request, CancellationToken cancellationToken)
    {
        var result = await _ratesValidFromService.GetValidityDates(request.CaseId, cancellationToken);

        return new GetInterestRatesValidFromResponse
        {
            InterestRatesValidFrom = 
            [
                new GetInterestRatesValidFromResponseItem 
                { 
                    InterestRateValidFrom = result.Date1.ToDateTime(new TimeOnly())
                },
                new GetInterestRatesValidFromResponseItem 
                { 
                    InterestRateValidFrom = result.Date2.ToDateTime(new TimeOnly()), 
                    IsDefault = true 
                }
            ]
        };
    }
}
