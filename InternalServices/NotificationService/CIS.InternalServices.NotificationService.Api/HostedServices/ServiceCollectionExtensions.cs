namespace CIS.InternalServices.NotificationService.Api.HostedServices;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBackgroundServices(this IServiceCollection services)
    {
        return services.AddHostedService<MscResultConsumer>();
    }
}