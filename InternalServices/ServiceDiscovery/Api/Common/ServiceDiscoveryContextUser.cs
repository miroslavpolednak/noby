namespace CIS.InternalServices.ServiceDiscovery.Api;

internal sealed record ServiceDiscoveryContextUser(int Id)
    : CIS.Core.Security.ICurrentUser
{
}
