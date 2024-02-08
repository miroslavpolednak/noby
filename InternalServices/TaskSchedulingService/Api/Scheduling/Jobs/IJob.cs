namespace CIS.InternalServices.TaskSchedulingService.Api.Scheduling.Jobs;

internal interface IJob
{
    Task Execute(CancellationToken cancellationToken);
}
