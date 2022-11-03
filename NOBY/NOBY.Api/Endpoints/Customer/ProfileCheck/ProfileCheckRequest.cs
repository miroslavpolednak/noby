namespace NOBY.Api.Endpoints.Customer.ProfileCheck;

public sealed record ProfileCheckRequest(long IdentityId, CIS.Foms.Enums.IdentitySchemes IdentityScheme)
    : IRequest<ProfileCheckResponse>
{
}
