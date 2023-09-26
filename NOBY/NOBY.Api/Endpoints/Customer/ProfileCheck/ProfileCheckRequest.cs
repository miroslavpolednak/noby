namespace NOBY.Api.Endpoints.Customer.ProfileCheck;

public sealed record ProfileCheckRequest(long IdentityId, SharedTypes.Enums.IdentitySchemes IdentityScheme)
    : IRequest<ProfileCheckResponse>
{
}
