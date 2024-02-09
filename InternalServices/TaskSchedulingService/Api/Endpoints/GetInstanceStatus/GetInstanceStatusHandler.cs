using CIS.InternalServices.TaskSchedulingService.Api.Scheduling.InstanceLocking;
using CIS.InternalServices.TaskSchedulingService.Contracts;

namespace CIS.InternalServices.TaskSchedulingService.Api.Endpoints.GetInstanceStatus;

internal sealed class GetInstanceStatusHandler
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

    private readonly Scheduling.InstanceLocking.ScheduleInstanceLockStatusService _lockService;

    public GetInstanceStatusHandler(ScheduleInstanceLockStatusService lockService)
    {
        _lockService = lockService;
    }
}
