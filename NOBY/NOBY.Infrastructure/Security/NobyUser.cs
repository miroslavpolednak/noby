using System.Security.Claims;
using System.Security.Principal;

namespace NOBY.Infrastructure.Security;

public class NobyUser
    : ClaimsPrincipal, CIS.Core.Security.ICurrentUser
{
    public int Id { get; init; }
    public string Login { get; init; }
    
    public NobyUser(IIdentity identity, int userId)
        : base(identity) 
    {
        Id = userId;
        Login = ((ClaimsIdentity)identity).Claims.FirstOrDefault(t => t.Type == CIS.Core.Security.SecurityConstants.ClaimTypeIdent)!.Value;
    }
}
