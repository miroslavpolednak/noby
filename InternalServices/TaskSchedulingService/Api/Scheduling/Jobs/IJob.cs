namespace CIS.InternalServices.TaskSchedulingService.Api.Scheduling.Jobs;

internal interface IJob
{
    Task Execute(string? jobData, CancellationToken cancellationToken);
}
