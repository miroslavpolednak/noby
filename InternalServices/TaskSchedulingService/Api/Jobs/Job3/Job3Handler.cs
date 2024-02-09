using CIS.Core.Attributes;
using CIS.InternalServices.TaskSchedulingService.Api.Scheduling.Jobs;

namespace CIS.InternalServices.TaskSchedulingService.Api.Jobs.Job3;

[ScopedService, SelfService]
internal sealed class Job3Handler
    : IJob
{
    private readonly ILogger<Job3Handler> _logger;

    public Job3Handler(ILogger<Job3Handler> logger)
    {
        _logger = logger;
    }

    public async Task Execute(CancellationToken cancellationToken)
    {
        _logger.LogInformation("3 LOGGER START");
        await Task.Delay(9000);
        _logger.LogInformation("3 LOGGER END");
    }
}
