using DomainServices.OfferService.Contracts;

namespace DomainServices.OfferService.Api.Endpoints.v1.GetInterestRate;

internal sealed class GetInterestRateHandler(ExternalServices.SbWebApi.V1.ISbWebApiClient _sbWebApi)
        : IRequestHandler<GetInterestRateRequest, GetInterestRateResponse>
{
    public async Task<GetInterestRateResponse> Handle(GetInterestRateRequest request, CancellationToken cancellationToken)
    {
        var result = await _sbWebApi.GetRefixationInterestRate(request.CaseId, request.FutureInterestRateValidTo, request.FixedRatePeriod, cancellationToken);

        return new GetInterestRateResponse
        {
            LoanInterestRate = result.InterestRate
        };
    }
}
