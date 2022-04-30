using System.Security.Claims;

namespace CIS.DomainServicesSecurity.ContextUser;

//TODO dodelat props az bude nadefinovana UserService
internal sealed class CisUserIdentity
    : ClaimsIdentity, Core.Security.ICurrentUser
{
    private readonly int _id;
    private readonly string _name;

    public CisUserIdentity(DomainServices.UserService.Contracts.User userInstance)
        : base(InternalServicesAuthentication.ContextUserSchemeName, ClaimTypes.NameIdentifier, ClaimTypes.Role)
    {
        this.AddClaim(new Claim(ClaimTypes.NameIdentifier, userInstance.Id.ToString(System.Globalization.CultureInfo.InvariantCulture)));
        this.AddClaim(new Claim(ClaimTypes.Name, userInstance.FullName));

        _name = userInstance.FullName;
        _id = userInstance.Id;
    }

    public int Id => _id;

    public string? DisplayName => _name;
}
