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
#pragma warning disable CA1848 // Use the LoggerMessage delegates
        _logger.LogInformation("LOGGER job1 START - {Data}", jobData);
        await Task.Delay(4000, cancellationToken);
        _logger.LogInformation("LOGGER job1 END");
#pragma warning restore CA1848 // Use the LoggerMessage delegates
    }
}
