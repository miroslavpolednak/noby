using System.Security.Claims;

namespace CIS.Infrastructure.Security.ContextUser;

internal sealed class CisUserIdentity
    : ClaimsIdentity, Core.Security.ICurrentUser
{
    private readonly int _id;

    public CisUserIdentity(int userId)
        : base(InternalServicesAuthentication.ContextUserSchemeName, ClaimTypes.NameIdentifier, ClaimTypes.Role)
    {
        _id = userId;
        this.AddClaim(new Claim(ClaimTypes.NameIdentifier, userId.ToString(System.Globalization.CultureInfo.InvariantCulture)));
    }

    public int Id => _id;
}
