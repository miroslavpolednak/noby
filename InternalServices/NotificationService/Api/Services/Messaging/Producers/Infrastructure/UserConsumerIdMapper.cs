using CIS.Core.Attributes;
using CIS.Core.Security;
using CIS.InternalServices.NotificationService.Api.Configuration;
using Microsoft.Extensions.Options;

namespace CIS.InternalServices.NotificationService.Api.Services.Messaging.Producers.Infrastructure;

[ScopedService, SelfService]
public class UserConsumerIdMapper
{
    private readonly IServiceUserAccessor _userAccessor;
    private Dictionary<string, string> _map;

    public UserConsumerIdMapper(
        IServiceUserAccessor userAccessor,
        IOptions<AppConfiguration> options)
    {
        _userAccessor = userAccessor;
        _map = options.Value.UserConsumerIdMap;
    }

    public string GetConsumerId()
    {
        var username = _userAccessor.User?.Name;

        if (string.IsNullOrEmpty(username) || !_map.ContainsKey(username))
        {
            // todo: cis error
            throw new ArgumentException(username);
        }

        return _map[username];
    }
}