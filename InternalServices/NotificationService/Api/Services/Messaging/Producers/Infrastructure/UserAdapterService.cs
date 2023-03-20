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
    private readonly Dictionary<string, Consumer> _consumers;

    public UserAdapterService(
        IServiceUserAccessor userAccessor,
        IOptions<AppConfiguration> options)
    {
        _userAccessor = userAccessor;
        _consumers = options.Value.Consumers.ToDictionary(c => c.Username);
    }

    public string GetUsername()
    {
        var username = _userAccessor.User?.Name;

        if (string.IsNullOrEmpty(username))
        {
            throw new CisAuthenticationException();
        }

        if (!_consumers.ContainsKey(username))
        {
            throw new CisAuthorizationException($"Forbidden for username '{username}'.");
        }

        return username;
    }
    
    public string GetConsumerId()
    {
        var username = GetUsername();

        return _consumers[username].ConsumerId;
    }

    public UserAdapterService CheckSendEmailAccess()
    {
        var username = GetUsername();
        if (!_consumers[username].CanSendEmail)
        {
            throw new CisAuthorizationException($"Forbidden for username '{username}'.");
        }
        
        return this;
    }

    public UserAdapterService CheckSendSmsAccess()
    {
        var username = GetUsername();
        if (!_consumers[username].CanSendSms)
        {
            throw new CisAuthorizationException($"Forbidden for username '{username}'.");
        }
        
        return this;
    }

    public UserAdapterService CheckReadResultAccess()
    {
        var username = GetUsername();
        if (!_consumers[username].CanReadResult)
        {
            throw new CisAuthorizationException($"Forbidden for username '{username}'.");
        }

        return this;
    }
}