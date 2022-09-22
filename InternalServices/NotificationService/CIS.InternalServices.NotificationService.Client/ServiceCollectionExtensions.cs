using CIS.InternalServices.NotificationService.Client.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace CIS.InternalServices.NotificationService.Client;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddNotificationClient(this IServiceCollection serviceCollection)
    {
        return serviceCollection
            .AddScoped<INotificationClient, Services.NotificationClient>();
    }
}