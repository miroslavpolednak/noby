using NOBY.Services.InterestRatesValidFrom;

namespace NOBY.Api.Endpoints.Refinancing.GetInterestRatesValidFrom;

internal sealed class GetInterestRatesValidFromHandler(InterestRatesValidFromService _ratesValidFromService)
    : IRequestHandler<GetInterestRatesValidFromRequest, RefinancingGetInterestRatesValidFromResponse>
{
    public async Task<RefinancingGetInterestRatesValidFromResponse> Handle(GetInterestRatesValidFromRequest request, CancellationToken cancellationToken)
    {
        var (date1, date2) = await _ratesValidFromService.GetValidityDates(request.CaseId, cancellationToken);

        return new()
        {
            InterestRatesValidFrom = 
            [
                new RefinancingGetInterestRatesValidFromResponseItem
                { 
                    InterestRateValidFrom = date1
                },
                new RefinancingGetInterestRatesValidFromResponseItem
                { 
                    InterestRateValidFrom = date2, 
                    IsDefault = true 
                }
            ]
        };
    }
}
