using CIS.InternalServices.NotificationService.Abstraction.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace CIS.InternalServices.NotificationService.Abstraction;

public static class NotificationServiceExtensions
{
    public static IServiceCollection AddNotificationServices(this IServiceCollection serviceCollection)
    {
        return serviceCollection
            .AddScoped<INotificationService, Services.NotificationService>();
    }
}