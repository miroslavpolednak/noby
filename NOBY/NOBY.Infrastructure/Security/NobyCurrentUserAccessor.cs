﻿using CIS.Core.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

namespace NOBY.Infrastructure.Security;

public sealed class NobyCurrentUserAccessor(IHttpContextAccessor _httpContext)
        : ICurrentUserAccessor
{
    private ICurrentUser? _user;
    private ICurrentUserDetails? _userDetails;
    private bool _userDetailsFetched;

    public ICurrentUser? User
    {
        get
        {
            _user ??= _httpContext.HttpContext?.User as ICurrentUser;
            return _user;
        }
    }

    public IEnumerable<Claim> Claims
    {
#pragma warning disable CS8603 // Possible null reference return.
        get => _httpContext!.HttpContext?.User.Claims;
#pragma warning restore CS8603 // Possible null reference return.
    }

    public bool IsAuthenticated => User is not null;

    public ICurrentUserDetails? UserDetails
    {
        get => _userDetailsFetched ? _userDetails : throw new InvalidOperationException("Trying to access UserDetails without fetching details first. Call FetchDetails() to ensure data being loaded.");
    }

    public async Task<ICurrentUserDetails> EnsureDetails(CancellationToken cancellationToken = default)
    {
        if (!IsAuthenticated)
            throw new InvalidOperationException("Missing authenticated user - can not fetch user details");
        if (_userDetailsFetched) return _userDetails!;

        _userDetailsFetched = true;

        var userService = _httpContext.HttpContext!.RequestServices.GetRequiredService<DomainServices.UserService.Clients.v1.IUserServiceClient>();
        var userInstance = await userService.GetUser(_user!.Id, cancellationToken);
        _userDetails = new CisUserDetails
        {
            DisplayName = userInstance.UserInfo.DisplayName
        };

        return _userDetails;
    }

    public async Task<TDetails> EnsureDetails<TDetails>(CancellationToken cancellationToken = default)
        where TDetails : ICurrentUserDetails
    {
        await EnsureDetails(cancellationToken);
        return (TDetails)_userDetails!;
    }
}
