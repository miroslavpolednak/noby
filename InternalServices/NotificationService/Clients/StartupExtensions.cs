using CIS.Infrastructure.gRPC;
using CIS.InternalServices;
using Microsoft.Extensions.DependencyInjection;
using __Contracts = CIS.InternalServices.NotificationService.Contracts;
using Microsoft.Extensions.DependencyInjection.Extensions;
using CIS.InternalServices.NotificationService.Clients.Services;

namespace DomainServices;

public static class StartupExtensions
{
    /// <summary>
    /// Service SD key
    /// </summary>
    public const string ServiceName = "CIS:NotificationService";

    public static IServiceCollection AddNotificationService(this IServiceCollection services)
    {
        services.AddCisServiceDiscovery();
        services.TryAddScoped<CIS.InternalServices.NotificationService.Clients.v2.INotificationServiceClient, NotificationServiceClientV2>();
        services.TryAddCisGrpcClientUsingServiceDiscovery<__Contracts.v2.NotificationService.NotificationServiceClient>(ServiceName);
        return services;
    }

    public static IServiceCollection AddCaseService(this IServiceCollection services, string serviceUrl)
    {
        services.TryAddScoped<CIS.InternalServices.NotificationService.Clients.v2.INotificationServiceClient, NotificationServiceClientV2>();
        services.TryAddCisGrpcClientUsingUrl<__Contracts.v2.NotificationService.NotificationServiceClient>(serviceUrl);
        return services;
    }
}
