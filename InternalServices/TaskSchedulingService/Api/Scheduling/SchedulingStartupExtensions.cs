using NCrontab.Scheduler;

namespace CIS.InternalServices.TaskSchedulingService.Api.Scheduling;

internal static class SchedulingStartupExtensions
{
    public static IServiceCollection AddSchedulingServices(this IServiceCollection services)
    {
        // cron scheduler
        services.AddSingleton<IScheduler>(services =>
        {
            return new Scheduler(
                services.GetRequiredService<ILogger<Scheduler>>(),
                new SchedulerOptions
                {
                    DateTimeKind = DateTimeKind.Local
                });
        });

        // schedule locking
        services.AddSingleton<InstanceLocking.ScheduleInstanceLockStatusService>();
        services.AddHostedService<InstanceLocking.ScheduleInstanceLockBackgroundService>();

        // job execution
        services.AddHostedService<SchedulerHostedService>();
        services.AddTransient<TriggerService>();

        return services;
    }
}
