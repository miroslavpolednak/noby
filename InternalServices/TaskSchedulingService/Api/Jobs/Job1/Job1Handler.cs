using CIS.Core.Attributes;
using CIS.InternalServices.TaskSchedulingService.Api.Scheduling.Jobs;

namespace CIS.InternalServices.TaskSchedulingService.Api.Jobs.Job1;

internal sealed class Job1Handler
    : IJob
{
    private readonly ILogger<Job1Handler> _logger;

    public Job1Handler(ILogger<Job1Handler> logger)
    {
        _logger = logger;
    }

    public async Task Execute(string? jobData, CancellationToken cancellationToken)
    {
        _logger.LogInformation("LOGGER START - {Data}", jobData);
        await Task.Delay(4000);
        _logger.LogInformation("LOGGER END");
    }
}
