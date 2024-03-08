using CIS.InternalServices.TaskSchedulingService.Contracts;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace CIS.InternalServices.TaskSchedulingService.Api.Endpoints;

internal sealed class TaskSchedulingService
    : Contracts.v1.TaskSchedulingService.TaskSchedulingServiceBase
{
    public override async Task<GetJobsResponse> GetJobs(GetJobsRequest request, ServerCallContext context)
        => await _mediator.Send(request, context.CancellationToken);

    public override async Task<GetTriggersResponse> GetTriggers(GetTriggersRequest request, ServerCallContext context)
        => await _mediator.Send(request, context.CancellationToken);

    public override async Task<GetInstanceStatusResponse> GetInstanceStatus(GetInstanceStatusRequest request, ServerCallContext context)
        => await _mediator.Send(request, context.CancellationToken);

    public override async Task<ExecuteJobResponse> ExecuteJob(ExecuteJobRequest request, ServerCallContext context)
        => await _mediator.Send(request, context.CancellationToken);

    public override async Task<Empty> UpdateScheduler(UpdateSchedulerRequest request, ServerCallContext context)
        => await _mediator.Send(request, context.CancellationToken);

    public override async Task<GetJobStatusesResponse> GetJobStatuses(GetJobStatusesRequest request, ServerCallContext context)
        => await _mediator.Send(request, context.CancellationToken);

    private readonly IMediator _mediator;
    public TaskSchedulingService(IMediator mediator)
        => _mediator = mediator;
}
