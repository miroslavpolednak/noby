using CIS.InternalServices.TaskSchedulingService.Api.Scheduling.InstanceLocking;
using CIS.InternalServices.TaskSchedulingService.Api.Scheduling.Jobs;
using CIS.InternalServices.TaskSchedulingService.Contracts;

namespace CIS.InternalServices.TaskSchedulingService.Api.Endpoints.ExecuteJob;

internal sealed class ExecuteJobHandler
    : IRequestHandler<ExecuteJobRequest, ExecuteJobResponse>
{
    public Task<ExecuteJobResponse> Handle(ExecuteJobRequest request, CancellationToken cancellation)
    {
        if (!_lockService.CurrentState.IsLockAcquired)
        {
            throw ErrorCodeMapper.CreateValidationException(ErrorCodeMapper.InstanceIsNotActive, _lockService.CurrentState.LockOwnerInstanceName);
        }

        var result = _jobExecutor.EnqueueJob(Guid.Parse(request.JobId), null, request.JobData, cancellation);

        if (result.IsSucessful)
        {
            return Task.FromResult(new ExecuteJobResponse
            {
                TraceId = result.TraceId,
                ScheduleJobStatusId = result.ScheduleJobStatusId.ToString()
            });
        }
        else
        {
            throw new CIS.Core.Exceptions.CisValidationException(result.ErrorMessage!);
        }
    }

    private readonly JobExecutor _jobExecutor;
    private readonly Scheduling.InstanceLocking.ScheduleInstanceLockStatusService _lockService;

    public ExecuteJobHandler(ScheduleInstanceLockStatusService lockService, JobExecutor jobExecutor)
    {
        _lockService = lockService;
        _jobExecutor = jobExecutor;
    }
}
