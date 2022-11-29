namespace CIS.InternalServices.NotificationService.Api.Services.Mcs.Consumers.BackgroundServices;

public class LogmanResultWorker : BackgroundService
{
    private readonly IServiceProvider _provider;
    private readonly ILogger<LogmanResultWorker> _logger;

    public LogmanResultWorker(IServiceProvider provider, ILogger<LogmanResultWorker> logger)
    {
        _provider = provider;
        _logger = logger;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await RetryPolicies.ForeverRetryPolicy.ExecuteAsync(async () =>
        {
            try
            {
                using var scope = _provider.CreateScope();
                var consumer = scope.ServiceProvider.GetRequiredService<LogmanResultConsumer>();
                await consumer.ConsumeAsync(stoppingToken);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Consuming Logman Kafka failed.");
                throw;
            }
        });
    }
}