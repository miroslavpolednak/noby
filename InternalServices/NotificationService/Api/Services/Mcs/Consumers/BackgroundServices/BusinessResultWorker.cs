namespace CIS.InternalServices.NotificationService.Api.Services.Mcs.Consumers.BackgroundServices;

public class BusinessResultWorker : BackgroundService
{
    private readonly IServiceProvider _provider;
    private readonly ILogger<BusinessResultWorker> _logger;

    public BusinessResultWorker(IServiceProvider provider, ILogger<BusinessResultWorker> logger)
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
                var consumer = scope.ServiceProvider.GetRequiredService<BusinessResultConsumer>();
                await consumer.ConsumeAsync(stoppingToken);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Consuming Business Kafka failed.");
                throw;
            }
        });
    }
}