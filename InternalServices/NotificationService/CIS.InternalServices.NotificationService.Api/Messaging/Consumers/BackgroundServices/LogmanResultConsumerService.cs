namespace CIS.InternalServices.NotificationService.Api.Messaging.Consumers.BackgroundServices;

public class LogmanResultConsumerService : BackgroundService
{
    private readonly IServiceProvider _provider;

    public LogmanResultConsumerService(IServiceProvider provider)
    {
        _provider = provider;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = _provider.CreateScope();
        var consumer = scope.ServiceProvider.GetRequiredService<LogmanResultConsumer>();
        await consumer.ConsumeAsync(stoppingToken);
    }
}