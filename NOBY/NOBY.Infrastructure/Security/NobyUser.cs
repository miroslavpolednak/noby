using System.Security.Claims;
using System.Security.Principal;

namespace NOBY.Infrastructure.Security;

public class NobyUser
    : ClaimsPrincipal, CIS.Core.Security.ICurrentUser
{
    public int Id { get; init; }
    public string Login { get; init; }
    public string DisplayName { get; init; }

    public NobyUser(IIdentity identity, DomainServices.UserService.Contracts.User userInstance) 
        : base(identity) 
    {
        Id = userInstance.UserId;
        Login = ((ClaimsIdentity)identity).Claims.FirstOrDefault(t => t.Type == CIS.Core.Security.SecurityConstants.ClaimTypeIdent)!.Value;
        DisplayName = userInstance.UserInfo.DisplayName;
    }
}
