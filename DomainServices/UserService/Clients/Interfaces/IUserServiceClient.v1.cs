namespace DomainServices.UserService.Clients.v1;

public interface IUserServiceClient
{
    Task<Contracts.User> GetCurrentUser(CancellationToken cancellationToken = default);

    Task<Contracts.GetUserBasicInfoResponse> GetUserBasicInfo(int userId, CancellationToken cancellationToken = default);
    Task<Contracts.GetUserBasicInfoResponse> GetUserBasicInfoWithoutCache(int userId, CancellationToken cancellationToken = default);

    Task<Contracts.User> GetUser(string loginWithScheme, CancellationToken cancellationToken = default);

    Task<Contracts.User> GetUser(SharedTypes.Types.UserIdentity identity, CancellationToken cancellationToken = default);

    Task<Contracts.User> GetUser(int userId, CancellationToken cancellationToken = default);
    
    Task<int[]> GetUserPermissions(int userId, CancellationToken cancellationToken = default);
    Task<int[]> GetUserPermissionsWithoutCache(int userId, CancellationToken cancellationToken = default);

    Task<int[]> GetCurrentUserPermissions(CancellationToken cancellationToken = default);
    
    Task<Contracts.UserRIPAttributes> GetUserRIPAttributes(string identity, string identityScheme, CancellationToken cancellationToken = default);

	Task<Contracts.GetUserMortgageSpecialistResponse> GetUserMortgageSpecialist(int userId, CancellationToken cancellationToken = default);
}
