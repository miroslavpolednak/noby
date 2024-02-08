using NCrontab.Scheduler;

namespace CIS.InternalServices.TaskSchedulingService.Api.Scheduling;

internal static class SchedulingStartupExtensions
{
    public static IServiceCollection AddSchedulingServices(this IServiceCollection services)
    {
        services.AddSingleton<IScheduler>(services =>
        {
            return new Scheduler(
                services.GetRequiredService<ILogger<Scheduler>>(),
                new SchedulerOptions
                {
                    DateTimeKind = DateTimeKind.Local
                });
        });

        services.AddSingleton(services =>
        {
            return JobExecutor.CreateInstance(services);
        });

        services.AddHostedService<SchedulerHostedService>();

        return services;
    }
}
