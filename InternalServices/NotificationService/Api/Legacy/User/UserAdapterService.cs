using CIS.Core.Exceptions;
using CIS.Core.Security;
using CIS.InternalServices.NotificationService.Api.Configuration;
using CIS.InternalServices.NotificationService.Api.Services.User.Abstraction;

namespace CIS.InternalServices.NotificationService.Api.Services.User;

internal sealed class UserAdapterService : IUserAdapterService
{
    private readonly IServiceUserAccessor _userAccessor;
    private readonly AppConfiguration _appConfiguration;

    public UserAdapterService(
        IServiceUserAccessor userAccessor,
        AppConfiguration appConfiguration)
    {
        _userAccessor = userAccessor;
        _appConfiguration = appConfiguration;
    }

    public string GetUsername()
    {
        var username = _userAccessor.User?.Name;

        if (string.IsNullOrEmpty(username))
        {
            throw new CisAuthenticationException();
        }

        return username;
    }
    
    public string GetConsumerId()
    {
        return _appConfiguration.Consumers.First(t => t.Username == _userAccessor.User!.Name).ConsumerId;
    }

    public UserAdapterService CheckSendEmailAccess()
    {
        if (!_userAccessor.IsInRole(UserRoles.SendEmail))
        {
            throw new CisAuthorizationException($"Forbidden for username '{_userAccessor.User.Name}'.");
        }
        
        return this;
    }

    public UserAdapterService CheckSendSmsAccess()
    {
        if (!_userAccessor.IsInRole(UserRoles.SendSms))
        {
            throw new CisAuthorizationException($"Forbidden for username '{_userAccessor.User.Name}'.");
        }
        
        return this;
    }

    public UserAdapterService CheckReadResultAccess()
    {
        if (!_userAccessor.IsInRole(UserRoles.ReadResult))
        {
            throw new CisAuthorizationException($"Forbidden for username '{_userAccessor.User.Name}'.");
        }

        return this;
    }

    public UserAdapterService CheckReceiveStatisticsAccess()
    {
        if (!_userAccessor.IsInRole(UserRoles.ReceiveStatistics))
        {
            throw new CisAuthorizationException($"Forbidden for username '{_userAccessor.User.Name}'.");
        }

        return this;
    }

    public UserAdapterService CheckResendNotificationsAccess()
    {
        if (!_userAccessor.IsInRole(UserRoles.ResendNotifications))
        {
            throw new CisAuthorizationException($"Forbidden for username '{_userAccessor.User.Name}'.");
        }

        return this;
    }
}