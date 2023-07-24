namespace DomainServices.UserService.Clients;

public interface IUserServiceClient
{
    Task<Contracts.User> GetUser(string loginWithScheme, CancellationToken cancellationToken = default(CancellationToken));

    Task<Contracts.User> GetUser(int userId, CancellationToken cancellationToken = default(CancellationToken));

    Task<Contracts.User> GetUser(CIS.Foms.Types.UserIdentity identity, CancellationToken cancellationToken = default(CancellationToken));

    Task<int[]> GetUserPermissions(int userId, CancellationToken cancellationToken = default(CancellationToken));

    Task<Contracts.UserRIPAttributes> GetUserRIPAttributes(string identity, string identityScheme, CancellationToken cancellationToken = default(CancellationToken));
}
