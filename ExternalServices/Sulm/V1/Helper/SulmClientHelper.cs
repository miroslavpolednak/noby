using CIS.Core.Security;

namespace ExternalServices.Sulm.V1;

internal sealed class SulmClientHelper(
    DomainServices.UserService.Clients.v1.IUserServiceClient _userService,
    ICurrentUserAccessor _userAccessor,
    ISulmClient _sulmClient)
        : ISulmClientHelper
{
    public async Task StartUse(long kbCustomerId, string purposeCode, CancellationToken cancellationToken = default)
    {
        var identities = await getUserIdentities(cancellationToken);

        await _sulmClient.StartUse(kbCustomerId, purposeCode, identities, cancellationToken);
    }

    public async Task StopUse(long kbCustomerId, string purposeCode, CancellationToken cancellationToken = default)
    {
        var identities = await getUserIdentities(cancellationToken);

        await _sulmClient.StopUse(kbCustomerId, purposeCode, identities, cancellationToken);
    }

    private async Task<List<SharedTypes.Types.UserIdentity>?> getUserIdentities(CancellationToken cancellationToken)
    {
        if (!_userAccessor.IsAuthenticated)
        {
            return null;
        }

        var userInstance = await _userService.GetUser(_userAccessor.User!.Id, cancellationToken);
        return userInstance.UserIdentifiers
            .Select(t => (SharedTypes.Types.UserIdentity)t!)
            .ToList();
    }
}
