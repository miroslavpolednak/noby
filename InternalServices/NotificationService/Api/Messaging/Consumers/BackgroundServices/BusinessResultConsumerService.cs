namespace CIS.InternalServices.NotificationService.Api.Messaging.Consumers.BackgroundServices;

public class BusinessResultConsumerService : BackgroundService
{
    private readonly IServiceProvider _provider;

    public BusinessResultConsumerService(IServiceProvider provider)
    {
        _provider = provider;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = _provider.CreateScope();
        var consumer = scope.ServiceProvider.GetRequiredService<BusinessResultConsumer>();
        await consumer.ConsumeAsync(stoppingToken);
    }
}