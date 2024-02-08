namespace CIS.InternalServices.TaskSchedulingService.Api.Scheduling;

internal sealed class JobExecutor
{
    private readonly ILogger<JobExecutor> _logger;
    private readonly IServiceProvider _serviceProvider;
    
    public JobExecutor(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _logger = _serviceProvider.GetRequiredService<ILogger<JobExecutor>>();
    }

    public async Task EnqueueJob(Type jobType, Guid triggerId, CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();

        _logger.LogInformation("Enqueueing job {JobType} for trigger {TriggerId}", jobType, triggerId);

        var job = (IJob)scope.ServiceProvider.GetService(jobType)!;
        await job.Execute(cancellationToken);

        _logger.LogInformation("Job {JobType} for trigger {TriggerId} finished", jobType, triggerId);
    }
}
