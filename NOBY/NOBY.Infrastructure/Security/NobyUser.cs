using System.Security.Claims;
using System.Security.Principal;

namespace NOBY.Infrastructure.Security;

public class NobyUser
    : ClaimsPrincipal, CIS.Core.Security.ICurrentUser
{
    public int Id { get; init; }
    public string Login { get; init; }
    public string DisplayName { get; init; }

    public NobyUser(IIdentity identity, string login, DomainServices.UserService.Contracts.User userInstance) 
        : base(identity) 
    {
        Id = userInstance.Id;
        Login = login;
        DisplayName = userInstance.FullName;
    }
}
