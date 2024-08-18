using Google.Protobuf.Collections;
using LazyCache;
using Microsoft.EntityFrameworkCore;

namespace NOBY.Api.Endpoints.Users.GetLoggedInUser;

internal sealed class GetLoggedInUserHandler(
	Database.FeApiDbContext _dbContext,
    IAppCache _cache,
    CIS.Core.Security.ICurrentUserAccessor _userAccessor,
    DomainServices.UserService.Clients.IUserServiceClient _userService)
        : IRequestHandler<GetLoggedInUserRequest, UsersGetLoggedInUserResponse>
{
    public async Task<UsersGetLoggedInUserResponse> Handle(GetLoggedInUserRequest request, CancellationToken cancellationToken)
    {
		var userInstance = await _userService.GetUser(_userAccessor.User!.Id, cancellationToken);

        // token info
        var (tokenFound, sessionValidTo) = await ((NobyCurrentUserAccessor)_userAccessor).GetOAuth2TokenInfo();

        var response = new UsersGetLoggedInUserResponse
        {
            UserId = userInstance.UserId,
            UserInfo = new()
            {
                FirstName = userInstance.UserInfo.FirstName,
                LastName = userInstance.UserInfo.LastName,
                Cin = userInstance.UserInfo.Cin,
                Cpm = userInstance.UserInfo.Cpm,
                Icp = userInstance.UserInfo.Icp,
                EmailAddress = userInstance.UserInfo?.Email,
                PhoneNumber = userInstance.UserInfo?.PhoneNumber,
                IsUserVIP = userInstance.UserInfo?.IsUserVIP ?? false,
                IsInternal = userInstance.UserInfo?.IsInternal ?? false
            },
            UserIdentifiers = userInstance.UserIdentifiers.Select(t => (SharedTypesUserIdentity)t!).ToList(),
            UserPermissions = getPermissions(userInstance.UserPermissions),
            SessionValidTo = sessionValidTo
		};

        if (userInstance.UserInfo?.IsInternal ?? false)
        {
			await fillMortgageSpecialist(userInstance.UserId, response, cancellationToken);
		}

        return response;
    }

    private async Task fillMortgageSpecialist(int userId, UsersGetLoggedInUserResponse response, CancellationToken cancellationToken)
    {
        // specialista nemusi existovat
        try
        {
            var specialist = await _userService.GetUserMortgageSpecialist(userId, cancellationToken);

			if (specialist is not null)
			{
				response.MortgageSpecialist = new UsersGetLoggedInUserResponseMortgageSpecialist
				{
					FirstName = specialist.Firstname,
					LastName = specialist.Lastname,
					PhoneNumber = specialist.Phone,
					EmailAddress = specialist.Email
				};
			}
		}
        catch { }
	}

    private List<int>? getPermissions(RepeatedField<int> permissions)
    {
        var allowedPermissions = _cache.GetOrAdd(nameof(GetLoggedInUserHandler), () =>
        {
            return _dbContext.FeAvailableUserPermissions.AsNoTracking().Select(t => t.PermissionCode).ToArray();
        }, DateTime.Now.AddDays(1));

        return permissions.Intersect(allowedPermissions).ToList();
    }
}
