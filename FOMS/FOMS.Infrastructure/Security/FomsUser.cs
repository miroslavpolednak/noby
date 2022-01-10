using System.Security.Claims;
using System.Security.Principal;

namespace FOMS.Infrastructure.Security;

public class FomsUser
    : ClaimsPrincipal, CIS.Core.Security.ICurrentUser
{
    public int Id { get; init; }
    public string Name { get; init; }
    public string Login { get; init; }

    public FomsUser(IIdentity identity, DomainServices.UserService.Contracts.User userInstance) 
        : base(identity) 
    {
        Id = userInstance.Id;
        Name = userInstance.FullName;
        Login = userInstance.Login;
    }
}
