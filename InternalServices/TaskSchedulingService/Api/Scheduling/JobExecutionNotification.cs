using CIS.InternalServices.TaskSchedulingService.Api.Jobs.Job1;

namespace CIS.InternalServices.TaskSchedulingService.Api.Scheduling;

internal sealed class JobExecutionNotification
{
    private readonly IServiceProvider _serviceProvider;

    public JobExecutionNotification(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public void EnqueJob()
    {
        Task.Run(() =>
        {
            _serviceProvider.GetRequiredService<>
        })
    }
}
