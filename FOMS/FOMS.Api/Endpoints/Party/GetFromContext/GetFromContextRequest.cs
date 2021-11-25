namespace FOMS.Api.Endpoints.Party.Dto;

internal sealed class GetFromContextRequest
    : IRequest<GetFromContextResponse>
{ 
    public string Token { get; init; }

    public GetFromContextRequest(string token)
    {
        Token = token;
    }
}
