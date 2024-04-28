using CIS.InternalServices.TaskSchedulingService.Api.Scheduling.InstanceLocking;
using CIS.InternalServices.TaskSchedulingService.Contracts;

namespace CIS.InternalServices.TaskSchedulingService.Api.Endpoints.GetInstanceStatus;

internal sealed class GetInstanceStatusHandler(ScheduleInstanceLockStatusService _lockService)
    : IRequestHandler<GetInstanceStatusRequest, GetInstanceStatusResponse>
{
    public Task<GetInstanceStatusResponse> Handle(GetInstanceStatusRequest request, CancellationToken cancellation)
    {
        return Task.FromResult(new GetInstanceStatusResponse
        {
            IsActiveInstance = _lockService.CurrentState.IsLockAcquired,
            ActiveInstanceName = _lockService.CurrentState.LockOwnerInstanceName
        });
    }
}
