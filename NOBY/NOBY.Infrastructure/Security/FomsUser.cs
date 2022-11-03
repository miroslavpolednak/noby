using System.Security.Claims;
using System.Security.Principal;

namespace NOBY.Infrastructure.Security;

public class FomsUser
    : ClaimsPrincipal, CIS.Core.Security.ICurrentUser
{
    public int Id { get; init; }
    public string DisplayName { get; init; }

    public FomsUser(IIdentity identity, DomainServices.UserService.Contracts.User userInstance) 
        : base(identity) 
    {
        Id = userInstance.Id;
        DisplayName = userInstance.FullName;
    }
}
