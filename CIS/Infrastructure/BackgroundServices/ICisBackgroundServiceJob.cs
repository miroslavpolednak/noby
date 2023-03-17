namespace CIS.Infrastructure.BackgroundServices;

public interface ICisBackgroundServiceJob
{
    Task ExecuteJobAsync(CancellationToken cancellationToken);
}
