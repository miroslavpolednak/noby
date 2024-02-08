using System.Diagnostics;

namespace CIS.InternalServices.TaskSchedulingService.Api.Jobs.Job1;

internal sealed class Job1Handler
    : INotificationHandler<Job1Notification>
{
    private readonly ILogger<Job1Handler> _logger;

    public Job1Handler(ILogger<Job1Handler> logger)
    {
        _logger = logger;
    }

    public async Task Handle(Job1Notification notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("LOGGER START");
        await Task.Delay(4000);
        _logger.LogInformation("LOGGER END");
    }
}
