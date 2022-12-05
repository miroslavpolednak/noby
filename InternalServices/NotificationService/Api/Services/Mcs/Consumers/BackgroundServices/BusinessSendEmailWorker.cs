namespace CIS.InternalServices.NotificationService.Api.Services.Mcs.Consumers.BackgroundServices;

public class BusinessSendEmailWorker : BackgroundService
{
    private readonly IServiceProvider _provider;
    private readonly ILogger<BusinessSendEmailWorker> _logger;

    public BusinessSendEmailWorker(IServiceProvider provider, ILogger<BusinessSendEmailWorker> logger)
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
                var consumer = scope.ServiceProvider.GetRequiredService<BusinessSendEmailConsumer>();
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