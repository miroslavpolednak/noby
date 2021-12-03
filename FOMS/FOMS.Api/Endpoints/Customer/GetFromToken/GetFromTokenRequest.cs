namespace FOMS.Api.Endpoints.Customer.Dto;

internal sealed class GetFromTokenRequest
    : IRequest<GetFromTokenResponse>
{ 
    public string Token { get; init; }

    public GetFromTokenRequest(string token)
    {
        Token = token;
    }
}
