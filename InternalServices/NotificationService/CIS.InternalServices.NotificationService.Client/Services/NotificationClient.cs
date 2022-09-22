using CIS.InternalServices.NotificationService.Client.Interfaces;
using Microsoft.Extensions.Logging;

namespace CIS.InternalServices.NotificationService.Client.Services;

public class NotificationClient : INotificationClient
{
    private readonly ILogger<NotificationClient> _logger;

    public NotificationClient(ILogger<NotificationClient> logger)
    {
        _logger = logger;
    }
}