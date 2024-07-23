using CIS.InternalServices.TaskSchedulingService.Contracts;

namespace CIS.InternalServices.TaskSchedulingService.Api.Endpoints.ExecuteJob;

internal sealed class ExecuteJobHandler(IMediator _mediator)
    : IRequestHandler<ExecuteJobRequest, ExecuteJobResponse>
{
    public Task<ExecuteJobResponse> Handle(ExecuteJobRequest request, CancellationToken cancellation)
    {
        var statusId = Guid.NewGuid();

        var notification = new Scheduling.Jobs.JobRunnerNotification(statusId, Guid.Parse(request.JobId), null, request.JobData, false);

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        _mediator.Publish(notification, CancellationToken.None);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

        return Task.FromResult(new ExecuteJobResponse
        {
            ScheduleJobStatusId = statusId.ToString()
        });
    }
}
