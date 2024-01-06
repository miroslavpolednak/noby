using CIS.Core.Security;

namespace ExternalServices.Sulm.V1;

internal sealed class SulmClientHelper
    : ISulmClientHelper
{
    public async Task StartUse(long kbCustomerId, string purposeCode, CancellationToken cancellationToken = default(CancellationToken))
    {
        var identities = await getUserIdentities(cancellationToken);

        await _sulmClient.StartUse(kbCustomerId, identities, purposeCode, cancellationToken);
    }

    public async Task StopUse(long kbCustomerId, string purposeCode, CancellationToken cancellationToken = default(CancellationToken))
    {
        var identities = await getUserIdentities(cancellationToken);

        await _sulmClient.StopUse(kbCustomerId, identities, purposeCode, cancellationToken);
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

    private readonly DomainServices.UserService.Clients.IUserServiceClient _userService;
    private readonly ICurrentUserAccessor _userAccessor;
    private readonly ISulmClient _sulmClient;

    public SulmClientHelper(
        DomainServices.UserService.Clients.IUserServiceClient userService,
        ICurrentUserAccessor userAccessor, 
        ISulmClient sulmClient)
    {
        _userService = userService;
        _userAccessor = userAccessor;
        _sulmClient = sulmClient;
    }
}
