using DomainServices.UserService.Contracts;
using _CIS = CIS.Infrastructure.gRPC.CisTypes;

namespace DomainServices.UserService.Clients.Services;
public class MockUserService : IUserServiceClient
{
    public Task<Contracts.User> GetUser(string loginWithScheme, CancellationToken cancellationToken = default(CancellationToken))
    {
        return Task.FromResult(CreateUser());
    }

    public Task<User> GetUser(int userId, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(CreateUser());
    }

    public Task<User> GetUser(CIS.Foms.Types.UserIdentity identity, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(CreateUser());
    }

    public Task<int[]> GetUserPermissions(int userId, CancellationToken cancellationToken = default(CancellationToken))
    {
        return Task.FromResult(Array.Empty<int>());
    }

    private static User CreateUser(
        int id = 3048,
        string czechIdentificationNumber = "12345678",
        string fullName = "Filip Tůma",
        string CPM = "99614w",
        string identity = """{ "identity": "990614w", "identityScheme": "Mpad" }""",
        _CIS.UserIdentity.Types.UserIdentitySchemes dentityScheme = _CIS.UserIdentity.Types.UserIdentitySchemes.Mpad
        )
    {
        var user = new User
        {
            UserId = id,
            UserInfo = new UserInfoObject
            {
                Cpm = CPM,
                Icp = "",
                Cin = czechIdentificationNumber
            }
        };

        user.UserIdentifiers.Add(new _CIS.UserIdentity
        {
            Identity = identity,
            IdentityScheme = dentityScheme

        });

        return user;
    }

}
