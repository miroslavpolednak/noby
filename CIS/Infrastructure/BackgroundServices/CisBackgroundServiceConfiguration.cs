namespace CIS.Infrastructure.BackgroundServices;

internal sealed class CisBackgroundServiceConfiguration<TBackgroundService>
    : ICisBackgroundServiceConfiguration<TBackgroundService>
    where TBackgroundService : ICisBackgroundService
{
    public bool Disabled { get; set; }

    public string CronSchedule { get; set; } = string.Empty;
}
