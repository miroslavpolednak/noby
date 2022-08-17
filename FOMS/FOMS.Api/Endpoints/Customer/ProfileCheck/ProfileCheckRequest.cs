namespace FOMS.Api.Endpoints.Customer.ProfileCheck;

public sealed class ProfileCheckRequest
    : IRequest<ProfileCheckResponse>
{
    /// <summary>
    /// ID klienta v danem schematu
    /// </summary>
    public long IdentityId { get; set; }

    /// <summary>
    /// Schema ve kterem je klient ulozeny - Kb | Mp
    /// </summary>
    public CIS.Foms.Enums.IdentitySchemes IdentityScheme { get; set; }
}
