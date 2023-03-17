using NCrontab;

namespace CIS.Infrastructure.BackgroundServices;

internal sealed class CisBackgroundService<TBackgroundService>
    : BackgroundService
    where TBackgroundService : ICisBackgroundService
{
    private readonly ILogger<TBackgroundService> _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ICisBackgroundServiceConfiguration<TBackgroundService> _options;
    private readonly CrontabSchedule _crontab;
    private readonly string _serviceName;

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

    protected sealed override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (_options.Disabled)
        {
            _logger.BackgroundServiceIsDisabled(_serviceName);
            return;
        }

        // prvni run poustime rovnou / bez ohledu na nastaveni scheduleru
        // chceme to takhle? asi jo.
        while (!stoppingToken.IsCancellationRequested)
        {
            // No further services are started until ExecuteAsync becomes asynchronous, such as by calling await.
            // Avoid performing long, blocking initialization work in ExecuteAsync. Task.Yield is here to eliminate this problem
            // Another benefit is, that Tasks with high priority gonna be processed preferably.
            await Task.Yield();

            try
            {
                using var serviceScope = _serviceScopeFactory.CreateScope();
                var service = serviceScope.ServiceProvider.GetRequiredService<TBackgroundService>();

                await service.ExecuteJobAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.BackgroundServiceExecutionError(_serviceName, ex);
            }

            // vypocitat next run
            var next = _crontab.GetNextOccurrence(DateTime.Now);
            _logger.BackgroundServiceNextRun(_serviceName, next);

            var nextRun = (next - DateTime.Now).Milliseconds;
            await Task.Delay(nextRun, stoppingToken);
        }
    }

    private CrontabSchedule getCrontabSchedule()
    {
        var cron = CrontabSchedule.Parse(_options.CronSchedule, new CrontabSchedule.ParseOptions
        {
            IncludingSeconds = true
        });

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
