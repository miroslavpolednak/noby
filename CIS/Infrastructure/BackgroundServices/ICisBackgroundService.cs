namespace CIS.Infrastructure.BackgroundServices;

public interface ICisBackgroundService
{
    Task ExecuteJobAsync(CancellationToken cancellationToken);
}
