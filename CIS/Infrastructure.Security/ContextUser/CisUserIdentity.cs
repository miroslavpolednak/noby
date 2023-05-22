using CIS.Core.Security;
using System.Security.Claims;

namespace CIS.Infrastructure.Security.ContextUser;

internal sealed class CisUserIdentity
    : ClaimsIdentity, ICurrentUser
{
    public int Id { get; init; }
    public string? Login { get; init; }
    public string? DisplayName { get; init; }

    public CisUserIdentity(string? login, DomainServices.UserService.Contracts.User userInstance)
        : base(InternalServicesAuthentication.ContextUserSchemeName, SecurityConstants.ClaimTypeId, "role")
    {
        Id = userInstance.UserId;
        Login = login;
        DisplayName = userInstance.UserInfo.DisplayName;
    }
}
