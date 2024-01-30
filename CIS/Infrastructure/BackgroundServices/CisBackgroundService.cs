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
    private readonly IConfiguration _configuration;
    private readonly CrontabSchedule _crontab;
    private int _iteration;

    /// <summary>
    /// [s] // Time for getting job lock, if job in not capable get applock in this time, job gonna be terminated (SqlException with message Execution Timeout Expired)
    /// </summary>
    private const int _technicalTimeout = 2;

    private static readonly string _serviceName = typeof(TBackgroundService).Name;

    private static readonly ActivitySource _activitySource = new(typeof(BackgroundService).Name);

    public CisBackgroundService(
        ILogger<TBackgroundService> logger,
        IConfiguration configuration,
        IServiceScopeFactory serviceScopeFactory,
        ICisBackgroundServiceConfiguration<TBackgroundService> options
        )
    {
        _configuration = configuration;
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;

        // parse cron expr.
        _crontab = getCrontabSchedule(options);

        // log cron settings
        _logger.BackgroundServiceRegistered(_serviceName, getCronDescription(options) ?? "Unable to get CRON description");

        // subscribe to listener
        ActivitySource.AddActivityListener(new()
        {
            ShouldListenTo = _ => true,
            SampleUsingParentId = (ref ActivityCreationOptions<string> _) => ActivitySamplingResult.AllData,
            Sample = (ref ActivityCreationOptions<ActivityContext> _) => ActivitySamplingResult.AllData,
        });
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // connection for db lock
        string connectionString = _configuration.GetConnectionString(CIS.Core.CisGlobalConstants.DefaultConnectionStringKey) 
            ?? throw new NotSupportedException("defaut connection string for background service distributed lock required");
    
        while (!stoppingToken.IsCancellationRequested)
        {
            // No further services are started until ExecuteAsync becomes asynchronous, such as by calling await.
            // Avoid performing long, blocking initialization work in ExecuteAsync. Task.Yield is here to eliminate this problem
            // Another benefit is, that Tasks with high priority gonna be processed preferably.
            await Task.Yield();

            using (var activity = _activitySource.StartActivity(_serviceName))
            {
                _iteration++;
                await waitUntilNext(stoppingToken);
                
                try
                {
                    var distributedLock = new SqlDistributedLock(_serviceName, connectionString);
                    using (var handle = await distributedLock.TryAcquireAsync(timeout: TimeSpan.FromSeconds(_technicalTimeout), cancellationToken: stoppingToken))
                    {
                        // we acquired the lock
                        if (handle != null)
                        {
                            using (var serviceScope = _serviceScopeFactory.CreateScope())
                            {
                                _logger.BackgroundServiceTaskStarted(_serviceName, _iteration);

                                var service = serviceScope.ServiceProvider.GetRequiredService<TBackgroundService>();
                                await service.ExecuteJobAsync(stoppingToken);

                                _logger.BackgroundServiceTaskFinished(_serviceName, _iteration);
                            }

                            // Technical timeout, when job will execute very fast and the exception (Execution Timeout Expired) does not have time to be thrown out
                            await Task.Delay(TimeSpan.FromSeconds(_technicalTimeout + 5), stoppingToken);
                        }
                        else // someone else has it
                        {
                            _logger.ParallelJobTerminated(_serviceName, _iteration);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.BackgroundServiceExecutionError(_serviceName, _iteration, ex);
                }
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

    private static CrontabSchedule getCrontabSchedule(ICisBackgroundServiceConfiguration<TBackgroundService> options)
    {
        var cron = CrontabSchedule.TryParse(options.CronSchedule);

        return cron ?? throw new CIS.Core.Exceptions.CisConfigurationException(0, $"Cron expression '{options.CronSchedule}' can not be parsed");
    }

    private static string? getCronDescription(ICisBackgroundServiceConfiguration<TBackgroundService> options)
    {
        return CronExpressionDescriptor.ExpressionDescriptor.GetDescription(options.CronSchedule, new CronExpressionDescriptor.Options
        {
            DayOfWeekStartIndexZero = true,
            Use24HourTimeFormat = true
        });
    }
}
