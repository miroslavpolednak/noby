namespace DomainServices.RealEstateValuationService.ExternalServices.LuxpiService.TokenService;

internal sealed class TokenHttpHandler
    : DelegatingHandler
{
    private readonly string _apiKey;
    private readonly ITokenService _tokenService;

    public TokenHttpHandler(string apiKey, ITokenService tokenService)
    {
        _tokenService = tokenService;
        _apiKey = apiKey;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var token = _tokenService.GetToken(_apiKey, cancellationToken);

        request.Headers.Add("Authorization", $"Bearer {token}");

        return await base.SendAsync(request, cancellationToken);
    }
}
