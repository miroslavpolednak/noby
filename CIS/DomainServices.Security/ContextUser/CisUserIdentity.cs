using System.Security.Claims;

namespace CIS.DomainServices.Security.ContextUser;

internal sealed class CisUserIdentity
    : ClaimsIdentity, Core.Security.ICurrentUser
{
    private readonly int _id;
    private readonly string _name;

    public CisUserIdentity(int id, string displayName)
        : base(InternalServicesAuthentication.ContextUserSchemeName, ClaimTypes.NameIdentifier, ClaimTypes.Role)
    {
        this.AddClaim(new Claim(ClaimTypes.NameIdentifier, id.ToString(System.Globalization.CultureInfo.InvariantCulture)));
        this.AddClaim(new Claim(ClaimTypes.Name, displayName));

        _name = displayName;
        _id = id;
    }

    public int Id => _id;

    public string? DisplayName => _name;
}
