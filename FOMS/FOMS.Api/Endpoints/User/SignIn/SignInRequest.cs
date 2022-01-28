namespace FOMS.Api.Endpoints.User.Dto;

internal sealed class SignInRequest : IRequest
{
    public string? Login { get; set; }
}
