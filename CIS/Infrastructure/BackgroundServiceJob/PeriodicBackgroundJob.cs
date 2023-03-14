namespace CIS.Infrastructure.BackgroundServiceJob;

public class PeriodicBackgroundJob<TBackgroundService> : BackgroundService
    where TBackgroundService : IPeriodicBackgroundServiceJob
{
    private readonly ILogger<TBackgroundService> _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IPeriodicJobConfiguration<TBackgroundService> _options;

    public PeriodicBackgroundJob(
        ILogger<TBackgroundService> logger,
        IServiceScopeFactory serviceScopeFactory,
        IPeriodicJobConfiguration<TBackgroundService> options
        )
    {
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
        _options = options;
    }

    protected sealed override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (_options.ServiceDisabled)
        {
            _logger.LogWarning("Periodic background service '{ServiceName}' is disabled in configuration.", typeof(TBackgroundService).Name);
            return;
        }

        while (!stoppingToken.IsCancellationRequested)
        {
            // No further services are started until ExecuteAsync becomes asynchronous, such as by calling await.
            // Avoid performing long, blocking initialization work in ExecuteAsync. Task.Yield is here to eliminate this problem
            // Another benefit is, that Tasks with high priority gonna be processed preferably.
            await Task.Yield();

            try
            {
                await ExecuteJob(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in periodic background service service '{ServiceName}' execution loop.", typeof(TBackgroundService).Name);
            }

            await Task.Delay(_options.TickInterval, stoppingToken);
        }
    }

    private async Task ExecuteJob(CancellationToken stoppingToken)
    {
        using var serviceScope = _serviceScopeFactory.CreateScope();
        var service = serviceScope.ServiceProvider.GetRequiredService<TBackgroundService>();

        await BeforeJobExecution(typeof(TBackgroundService).Name);
        await service.ExecuteJobAsync(stoppingToken);
        await AfterJobExecution(typeof(TBackgroundService).Name);
    }

    protected virtual Task BeforeJobExecution(string jobName)
    {
        _logger.LogInformation("Starting execution of background service '{ServiceName}'", jobName);
        return Task.CompletedTask;
    }

    protected virtual Task AfterJobExecution(string jobName)
    {
        _logger.LogInformation("Ending the execution of a background service '{ServiceName}'", jobName);
        return Task.CompletedTask;
    }
}
