using System.Security.Claims;

namespace CIS.Security.InternalServices.ContextUser;

internal sealed class CisUserIdentity
    : ClaimsIdentity, Core.Security.ICurrentUser
{
    private readonly int _id;
    private readonly string _name;

    public CisUserIdentity(int id, string name, string displayName)
        : base("CisContextUser", ClaimTypes.NameIdentifier, ClaimTypes.Role)
    {
        this.AddClaim(new Claim(ClaimTypes.NameIdentifier, name));
        this.AddClaim(new Claim(ClaimTypes.Name, displayName));

        _name = name;
        _id = id;
    }

    public int Id => _id;

    public string? DisplayName => _name;
}
