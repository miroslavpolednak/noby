namespace CIS.InternalServices.TaskSchedulingService.Api.Scheduling;

internal interface IJob
{
    Task Execute(CancellationToken cancellationToken);
}
