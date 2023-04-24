using DomainServices.UserService.Contracts;
using _CIS = CIS.Infrastructure.gRPC.CisTypes;

namespace DomainServices.UserService.Clients.Services;
public class MockUserService : IUserServiceClient
{
    public Task<User> GetUser(int userId, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(CreateUser());
    }

    public Task<User> GetUserByLogin(string login, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(CreateUser());
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
            Id = id,
            CzechIdentificationNumber = czechIdentificationNumber,
            FullName = fullName,
            CPM = CPM,
        };

        user.UserIdentifiers.Add(new _CIS.UserIdentity
        {
            Identity = identity,
            IdentityScheme = dentityScheme

        });

        return user;
    }

}
