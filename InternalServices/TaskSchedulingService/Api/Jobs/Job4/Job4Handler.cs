using CIS.Core.Attributes;
using CIS.InternalServices.TaskSchedulingService.Api.Scheduling.Jobs;

namespace CIS.InternalServices.TaskSchedulingService.Api.Jobs.Job4;

[ScopedService, SelfService]
internal sealed class Job4Handler
    : IJob
{
    private readonly ILogger<Job4Handler> _logger;

    public Job4Handler(ILogger<Job4Handler> logger)
    {
        _logger = logger;
    }

    public async Task Execute(CancellationToken cancellationToken)
    {
        _logger.LogInformation("4 LOGGER START");
        await Task.Delay(3500);
        _logger.LogInformation("4 LOGGER END");
    }
}
