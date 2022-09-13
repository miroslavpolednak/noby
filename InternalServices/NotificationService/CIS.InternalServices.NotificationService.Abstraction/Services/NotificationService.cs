using CIS.InternalServices.NotificationService.Abstraction.Interfaces;
using Microsoft.Extensions.Logging;

namespace CIS.InternalServices.NotificationService.Abstraction.Services;

public class NotificationService : INotificationService
{
    private readonly ILogger<NotificationService> _logger;

    public NotificationService(ILogger<NotificationService> logger)
    {
        _logger = logger;
    }
}