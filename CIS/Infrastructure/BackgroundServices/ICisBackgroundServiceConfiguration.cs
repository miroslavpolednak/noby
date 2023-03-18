namespace CIS.Infrastructure.BackgroundServices;

internal interface ICisBackgroundServiceConfiguration<TBackgroundService>
    where TBackgroundService : ICisBackgroundServiceJob
{
    /// <summary>
    /// True pokud je job vypnuty a nema se spoustet
    /// </summary>
    bool Disabled { get; set; }

    /// <summary>
    /// Nastaveni schedule pro spousteni jobu
    /// </summary>
    string CronSchedule { get; set; }
}
