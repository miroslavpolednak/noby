using Confluent.Kafka.DependencyInjection;

namespace CIS.InternalServices.NotificationService.Api.BackgroundServices;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBackgroundServices(this IServiceCollection services)
    {
        return services
            .AddKafkaClient()
            .AddHostedService<ResultConsumer>();
    }
}