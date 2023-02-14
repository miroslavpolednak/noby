using CIS.Core.Attributes;
using CIS.Core.Exceptions;
using CIS.Core.Security;
using CIS.InternalServices.NotificationService.Api.Configuration;
using Microsoft.Extensions.Options;

namespace CIS.InternalServices.NotificationService.Api.Services.Messaging.Producers.Infrastructure;

[ScopedService, SelfService]
public class UserAdapterService
{
    private readonly IServiceUserAccessor _userAccessor;
    private Dictionary<string, string> _map;

    public UserAdapterService(
        IServiceUserAccessor userAccessor,
        IOptions<AppConfiguration> options)
    {
        _userAccessor = userAccessor;
        _map = options.Value.UserConsumerIdMap;
    }

    public string GetConsumerId()
    {
        var username = GetUsername();

        return _map[username];
    }

    public string GetUsername()
    {
        var username = _userAccessor.User?.Name;

        if (string.IsNullOrEmpty(username))
        {
            throw new CisAuthenticationException();
        }

        if (!_map.ContainsKey(username))
        {
            throw new CisAuthorizationException($"Forbidden for username '{username}'.");
        }

        return username;
    }
}