namespace CIS.Infrastructure.BackgroundServices;

/// <summary>
/// Obecna konfigurace background service.
/// </summary>
/// <typeparam name="TBackgroundService">Typ jobu pro ktery je konfigurace platna.</typeparam>
public interface ICisBackgroundServiceConfiguration<TBackgroundService>
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
