using CIS.Core.Security;
using System.Security.Claims;

namespace CIS.Infrastructure.Security.ContextUser;

internal sealed class CisUserIdentity
    : ClaimsIdentity, ICurrentUser
{
    public int Id { get; init; }

    public string? Login { get; init; }

    public CisUserIdentity(int userId, string? login)
        : base(InternalServicesAuthentication.ContextUserSchemeName, SecurityConstants.ClaimNameId, "role")
    {
        Id = userId;
        Login = login;

        this.AddClaim(new Claim(SecurityConstants.ClaimNameId, userId.ToString(System.Globalization.CultureInfo.InvariantCulture)));
        if (!string.IsNullOrEmpty(login))
        {
            this.AddClaim(new Claim(SecurityConstants.ClaimNameIdent, login));
        }
    }
}
