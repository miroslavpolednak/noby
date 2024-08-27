using DomainServices.UserService.Clients.Dto;
using DomainServices.UserService.Contracts;
using _CIS = SharedTypes.GrpcTypes;

namespace DomainServices.UserService.Clients.v1;

public class MockUserServiceClient
    : IUserServiceClient
{
    public Task<UserDto> GetUser(string loginWithScheme, CancellationToken cancellationToken = default(CancellationToken))
    {
        return Task.FromResult(CreateUser());
    }

    public Task<UserDto> GetUser(int userId, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(CreateUser());
    }

    public Task<UserDto> GetUser(SharedTypes.Types.UserIdentity identity, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(CreateUser());
    }

    public Task<int[]> GetUserPermissions(int userId, CancellationToken cancellationToken = default(CancellationToken))
    {
        return Task.FromResult(Array.Empty<int>());
    }

    private static UserDto CreateUser(
        int id = DefaultUserId,
        string czechIdentificationNumber = "12345678",
        string fullName = "Filip Tůma",
        string cpm = "99614w",
        string icp = "",
        string identity = """{ "identity": "990614w", "identityScheme": "Mpad" }""",
        _CIS.UserIdentity.Types.UserIdentitySchemes identityScheme = _CIS.UserIdentity.Types.UserIdentitySchemes.Mpad
        )
    {
        var user = new UserDto
        {
            UserId = id,
            UserInfo = new UserInfoObject
            {
                Cpm = cpm,
                Icp = icp,
                FirstName = "Filip",
                LastName = "Tuma",
                Email = "a@mpss.cz",
                PhoneNumber = "999999999",
                DisplayName = fullName,
                Cin = czechIdentificationNumber
            },
            UserIdentifiers =
            [
                new()
                {
                    Identity = identity,
                    IdentityScheme = identityScheme

                }

            ]
        };

        return user;
    }

    public Task<UserRIPAttributes> GetUserRIPAttributes(string identity, string identityScheme, CancellationToken cancellationToken = default(CancellationToken))
    {
        return Task.FromResult(new UserRIPAttributes()
        {
            PersonId = 1,
            PersonSurname = "Surname"
        });
    }

    public async Task<UserDto> GetCurrentUser(CancellationToken cancellationToken = default)
        => await GetUser(1, cancellationToken);

    public Task<GetUserBasicInfoResponse> GetUserBasicInfo(int userId, CancellationToken cancellationToken = default)
    {
        var user = CreateUser();
        return Task.FromResult(new GetUserBasicInfoResponse
        {
            DisplayName = user.UserInfo.DisplayName
        });
    }

    public Task<int[]> GetCurrentUserPermissions(CancellationToken cancellationToken = default)
    {
        return GetUserPermissions(1, cancellationToken);
    }

    public Task<GetUserBasicInfoResponse> GetUserBasicInfoWithoutCache(int userId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<int[]> GetUserPermissionsWithoutCache(int userId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<GetUserMortgageSpecialistResponse> GetUserMortgageSpecialist(int userId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public const int DefaultUserId = 3048;
}
