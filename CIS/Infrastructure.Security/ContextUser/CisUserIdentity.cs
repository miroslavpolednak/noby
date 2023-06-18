using CIS.Core.Security;
using System.Security.Claims;

namespace CIS.Infrastructure.Security.ContextUser;

internal sealed class CisUserIdentity
    : ClaimsIdentity, ICurrentUser
{
    public int Id { get; init; }
    public string? Login { get; init; }
    
    public CisUserIdentity(int userId, string? login)
        : base(InternalServicesAuthentication.ContextUserSchemeName, SecurityConstants.ClaimTypeId, "role")
    {
        Id = userId;
        Login = login;
    }
}
