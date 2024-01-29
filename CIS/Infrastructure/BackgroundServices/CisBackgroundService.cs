using Medallion.Threading.SqlServer;
using Microsoft.EntityFrameworkCore;
using NCrontab;
using System.Diagnostics;

namespace CIS.Infrastructure.BackgroundServices;

internal sealed class CisBackgroundService<TBackgroundService>
    : BackgroundService
    where TBackgroundService : ICisBackgroundServiceJob
{
    private readonly ILogger<TBackgroundService> _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ICisBackgroundServiceConfiguration<TBackgroundService> _options;
    private readonly CrontabSchedule _crontab;
    private readonly string _serviceName;

    private static readonly ActivitySource _activitySource = new(typeof(BackgroundService).Name);

    private static readonly ActivityListener _activityListener = new()
    {
        ShouldListenTo = _ => true,
        SampleUsingParentId = (ref ActivityCreationOptions<string> _) => ActivitySamplingResult.AllData,
        Sample = (ref ActivityCreationOptions<ActivityContext> _) => ActivitySamplingResult.AllData,
    };

    /// <summary>
    /// [s] // Time for getting job lock, if job in not capable get applock in this time, job gonna be terminated (SqlException with message Execution Timeout Expired)
    /// </summary>
    const int _technicalTimeout = 2;

    public CisBackgroundService(
        ILogger<TBackgroundService> logger,
        IServiceScopeFactory serviceScopeFactory,
        ICisBackgroundServiceConfiguration<TBackgroundService> options
        )
    {
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
        _options = options;

        _serviceName = typeof(TBackgroundService).Name;

        // parse cron expr.
        _crontab = getCrontabSchedule();

        // log cron settings
        logServiceRegistered();

        // subscribe to listener
        ActivitySource.AddActivityListener(_activityListener);
    }

    public string? ConnectionString { get; set; }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var activity = _activitySource.StartActivity(typeof(TBackgroundService).Name);

        if (_options.Disabled)
        {
            _logger.BackgroundServiceIsDisabled(_serviceName);
            return;
        }

        while (!stoppingToken.IsCancellationRequested)
        {
            // No further services are started until ExecuteAsync becomes asynchronous, such as by calling await.
            // Avoid performing long, blocking initialization work in ExecuteAsync. Task.Yield is here to eliminate this problem
            // Another benefit is, that Tasks with high priority gonna be processed preferably.
            await Task.Yield();

            await waitUntilNext(stoppingToken);

            using var serviceScope = _serviceScopeFactory.CreateScope();
            var service = serviceScope.ServiceProvider.GetRequiredService<TBackgroundService>();
            var configuration = serviceScope.ServiceProvider.GetRequiredService<IConfiguration>();
            ConnectionString = ConnectionString is null ? configuration.GetConnectionString("default") ?? throw new NotSupportedException("defaut connection string required") : ConnectionString;
            // lockName is essential for uniquely identifying the resource for which the lock is being requested.
            string lockName = typeof(TBackgroundService)?.FullName!;
            try
            {
                var distributedLock = new SqlDistributedLock(lockName, ConnectionString);
                using (var handle = await distributedLock.TryAcquireAsync(timeout: TimeSpan.FromSeconds(_technicalTimeout), cancellationToken: stoppingToken))
                {
                    // we acquired the lock
                    if (handle != null)
                    {
                        await service.ExecuteJobAsync(stoppingToken);
                        // Technical timeout, when job will execute very fast and the exception (Execution Timeout Expired) does not have time to be thrown out
                        await Task.Delay(TimeSpan.FromSeconds(_technicalTimeout + 5), stoppingToken);
                    }
                    else // someone else has it
                    {
                        _logger.ParallelJobTerminated(lockName);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.BackgroundServiceExecutionError(_serviceName, ex);
            }
        }
    }

    private async Task waitUntilNext(CancellationToken stoppingToken)
    {
        // vypocitat next run
        var next = _crontab.GetNextOccurrence(DateTime.Now);
        var nextRun = Convert.ToInt32((next - DateTime.Now).TotalMilliseconds);
        if (nextRun < 1000)
        {
            await Task.Delay(1000, stoppingToken);
            await waitUntilNext(stoppingToken);
        }
        else
        {
            _logger.BackgroundServiceNextRun(_serviceName, next);
            await Task.Delay(nextRun, stoppingToken);
        }
    }

    private CrontabSchedule getCrontabSchedule()
    {
        var cron = CrontabSchedule.TryParse(_options.CronSchedule);

        return cron ?? throw new CIS.Core.Exceptions.CisConfigurationException(0, $"Cron expression '{_options.CronSchedule}' can not be parsed");
    }

    private void logServiceRegistered()
    {
        var cronDescription = CronExpressionDescriptor.ExpressionDescriptor.GetDescription(_options.CronSchedule, new CronExpressionDescriptor.Options
        {
            DayOfWeekStartIndexZero = true,
            Use24HourTimeFormat = true
        });
        _logger.BackgroundServiceRegistered(_serviceName, cronDescription);
    }
}
