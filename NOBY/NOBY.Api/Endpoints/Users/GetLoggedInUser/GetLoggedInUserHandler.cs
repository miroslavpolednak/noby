using Google.Protobuf.Collections;
using LazyCache;
using Microsoft.EntityFrameworkCore;

namespace NOBY.Api.Endpoints.Users.GetLoggedInUser;

internal sealed class GetLoggedInUserHandler
    : IRequestHandler<GetLoggedInUserRequest, GetLoggedInUserResponse>
{
    public async Task<GetLoggedInUserResponse> Handle(GetLoggedInUserRequest request, CancellationToken cancellationToken)
    {
        var userInstance = await _userService.GetUser(_userAccessor.User!.Id, cancellationToken);

        return new GetLoggedInUserResponse
        {
            UserId = userInstance.UserId,
            UserInfo = new GetLoggedInUserResponseUserInfo
            {
                FirstName = userInstance.UserInfo.FirstName,
                LastName = userInstance.UserInfo.LastName,
                Cin = userInstance.UserInfo.Cin,
                Cpm = userInstance.UserInfo.Cpm,
                Icp = userInstance.UserInfo.Icp
            },
            UserIdentifiers = userInstance.UserIdentifiers.Select(t => (CIS.Foms.Types.UserIdentity)t!).ToList(),
            UserAttributes = new GetLoggedInUserResponseAttributes
            {
                EmailAddress = userInstance.UserAttributes?.Email,
                PhoneNumber = userInstance.UserAttributes?.PhoneNumber,
                IsUserVIP = userInstance.UserAttributes?.IsUserVIP ?? false
            },
            UserPermissions = getPermissions(userInstance.UserPermissions)
        };
    }

    private int[]? getPermissions(RepeatedField<int> permissions)
    {
        var allowedPermissions = _cache.GetOrAdd(nameof(GetLoggedInUserHandler), () =>
        {
            return _dbContext.FeAvailableUserPermissions.AsNoTracking().Select(t => t.PermissionCode).ToArray();
        }, DateTime.Now.AddDays(1));

        return permissions.Intersect(allowedPermissions).ToArray();
    }

    private readonly IAppCache _cache;
    private readonly Database.FeApiDbContext _dbContext;
    private readonly DomainServices.UserService.Clients.IUserServiceClient _userService;
    private readonly CIS.Core.Security.ICurrentUserAccessor _userAccessor;

    public GetLoggedInUserHandler(
        Database.FeApiDbContext dbContext,
        IAppCache cache,
        CIS.Core.Security.ICurrentUserAccessor userAccessor,
        DomainServices.UserService.Clients.IUserServiceClient userService)
    {
        _dbContext = dbContext;
        _cache = cache;
        _userAccessor = userAccessor;
        _userService = userService;
    }
}
