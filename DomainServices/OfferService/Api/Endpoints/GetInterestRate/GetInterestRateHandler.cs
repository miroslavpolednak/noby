using DomainServices.OfferService.Contracts;

namespace DomainServices.OfferService.Api.Endpoints.GetInterestRate;

internal sealed class GetInterestRateHandler
    : IRequestHandler<GetInterestRateRequest, GetInterestRateResponse>
{
    public async Task<GetInterestRateResponse> Handle(GetInterestRateRequest request, CancellationToken cancellationToken)
    {
        var result = await _sbWebApi.GetRefixationInterestRate(request.CaseId, request.FutureInterestRateValidTo, cancellationToken);

        return new GetInterestRateResponse
        {
            LoanInterestRate = result.InterestRate
        };
    }

    private readonly ExternalServices.SbWebApi.V1.ISbWebApiClient _sbWebApi;

    public GetInterestRateHandler(ExternalServices.SbWebApi.V1.ISbWebApiClient sbWebApi)
    {
        _sbWebApi = sbWebApi;
    }
}
