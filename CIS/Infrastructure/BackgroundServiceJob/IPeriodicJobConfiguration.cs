namespace CIS.Infrastructure.BackgroundServiceJob;

public interface IPeriodicJobConfiguration<TBackgroundService>
    where TBackgroundService : IPeriodicBackgroundServiceJob
{
    public string SectionName { get; }

    bool ServiceDisabled { get; set; }

    TimeSpan TickInterval { get; set; }
}
