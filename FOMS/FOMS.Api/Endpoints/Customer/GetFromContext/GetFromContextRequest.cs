namespace FOMS.Api.Endpoints.Customer.Dto;

internal sealed class GetFromContextRequest
    : IRequest<GetFromContextResponse>
{ 
    public string Token { get; init; }

    public GetFromContextRequest(string token)
    {
        Token = token;
    }
}
