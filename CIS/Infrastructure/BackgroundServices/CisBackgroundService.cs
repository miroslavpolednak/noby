using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using NCrontab;
using System.Data;

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

    /// <summary>
    /// [s] // Time for getting job lock, if job in not capable get applock in this time, job gonna be terminated (SqlException with message Execution Timeout Expired)
    /// </summary>
    const int _technicalTimeout = 2;
    const string _lockMode = "Exclusive";
    const string _lockOwner = "Session";

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

    }

    public string? ConnectionString { get; set; }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
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
            
            // resource is essential for uniquely identifying the resource for which the lock is being requested.
            string resource = typeof(TBackgroundService)?.FullName!;
            var canJobRun = false;

            using var connection = new SqlConnection(ConnectionString);
            await connection.OpenAsync(stoppingToken);

            try
            {
                canJobRun = await getAppLock(connection, resource, _lockMode, _lockOwner);
                if (canJobRun)
                {
                    await service.ExecuteJobAsync(stoppingToken);
                }
            }
            catch (SqlException ex) when (ex.Message.Contains("Execution Timeout Expired"))
            {
                _logger.ParallelJobTerminated(resource);
            }
            catch (Exception ex)
            {
                _logger.BackgroundServiceExecutionError(_serviceName, ex);
            }
            finally
            {
                if (canJobRun)
                {
                    // Technical timeout, when job will execute very fast and the exception (Execution Timeout Expired) does not have time to be thrown out
                    await Task.Delay(TimeSpan.FromSeconds(_technicalTimeout), stoppingToken);
                    await releaseLock(connection, resource, _lockOwner);
                }

                await connection.CloseAsync();
            }
        }
    }

    public async Task<bool> getAppLock(SqlConnection connection, string resource, string lockMode, string lockOwner)
    {
        // Acquire the lock
        var result = await connection.QueryFirstOrDefaultAsync<int>(
            "sp_getapplock",
            new { Resource = resource, LockMode = lockMode, LockOwner = lockOwner },
            commandType: CommandType.StoredProcedure,
            commandTimeout: _technicalTimeout);

        if (result < 0)
        {
            //-1   The lock request timed out.
            //-2   The lock request was canceled.
            //-3   The lock request was chosen as a deadlock victim.
            //-999 Indicates a parameter validation or other call error.
            _logger.DbCannotSetAppLock(result);
        }

        return result >= 0;
    }

    public static async Task releaseLock(SqlConnection connection, string resource, string lockOwner) =>
        await connection.ExecuteAsync(
                     "sp_releaseapplock",
                     new { Resource = resource, LockOwner = lockOwner },
                     commandType: CommandType.StoredProcedure);

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
