namespace CIS.InternalServices.TaskSchedulingService.Api.Scheduling.Jobs;

internal record JobRunnerNotification(Guid JobStatusId, Guid JobId, Guid? TriggerId, string? JobData, bool ActiveInstanceCheck)
    : INotification
{
}
