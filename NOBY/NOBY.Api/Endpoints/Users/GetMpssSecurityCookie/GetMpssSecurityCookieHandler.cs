﻿using CIS.Core.Security;

namespace NOBY.Api.Endpoints.Users.GetMpssSecurityCookie;

internal sealed class GetMpssSecurityCookieHandler(
	MPSS.Security.Noby.IPortal _portal,
	DomainServices.UserService.Clients.v1.IUserServiceClient _userServiceClient,
	ICurrentUserAccessor _currentUser)
		: IRequestHandler<GetMpssSecurityCookieRequest, string>
{
    public async Task<string> Handle(GetMpssSecurityCookieRequest request, CancellationToken cancellationToken)
    {
        var u = await _userServiceClient.GetUser(_currentUser.User!.Id, cancellationToken);

        string? m17id = u.UserIdentifiers.FirstOrDefault(t => t.IdentityScheme == SharedTypes.GrpcTypes.UserIdentity.Types.UserIdentitySchemes.M17Id)?.Identity;
        string? brokerId = u.UserIdentifiers.FirstOrDefault(t => t.IdentityScheme == SharedTypes.GrpcTypes.UserIdentity.Types.UserIdentitySchemes.BrokerId)?.Identity;
        string? kbuid = u.UserIdentifiers.FirstOrDefault(t => t.IdentityScheme == SharedTypes.GrpcTypes.UserIdentity.Types.UserIdentitySchemes.KbUid)?.Identity;

#pragma warning disable CA1305 // Specify IFormatProvider
        var cookieValue = _portal.CreateCookieValue(u.UserInfo.Cpm, u.UserInfo.Icp, u.UserInfo.DisplayName, u.UserId, 0, 0, 0, string.IsNullOrEmpty(m17id) ? 0 : Convert.ToInt32(m17id), string.IsNullOrEmpty(brokerId) ? 0 : Convert.ToInt32(brokerId), kbuid ?? "", u.UserInfo.ChannelId);
#pragma warning restore CA1305 // Specify IFormatProvider

        return cookieValue;
    }
}
