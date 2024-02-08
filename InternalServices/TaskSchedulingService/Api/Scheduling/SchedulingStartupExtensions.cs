using NCrontab.Scheduler;

namespace CIS.InternalServices.TaskSchedulingService.Api.Scheduling;

internal static class SchedulingStartupExtensions
{
    public static IServiceCollection AddSchedulingServices(this IServiceCollection services)
    {
        services.AddSingleton<IScheduler>(x =>
        {
            return new Scheduler(
                x.GetRequiredService<ILogger<Scheduler>>(),
                new SchedulerOptions
                {
                    DateTimeKind = DateTimeKind.Local
                });
        });

        services
            .AddHostedService<SchedulerHostedService>();

        return services;
    }
}
