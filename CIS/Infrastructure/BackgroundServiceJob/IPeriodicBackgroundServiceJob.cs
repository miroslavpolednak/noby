namespace CIS.Infrastructure.BackgroundServiceJob;

public interface IPeriodicBackgroundServiceJob
{
    Task ExecuteJobAsync(CancellationToken cancellationToken);
}