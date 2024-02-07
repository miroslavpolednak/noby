using CIS.InternalServices.TaskSchedulingService.Contracts;
using Grpc.Core;

namespace CIS.InternalServices.TaskSchedulingService.Api.Endpoints;

internal sealed class TaskSchedulingService
    : Contracts.v1.TaskSchedulingService.TaskSchedulingServiceBase
{
    public override async Task<GetAvailableTasksResponse> GetAvailableTasks(GetAvailableTasksRequest request, ServerCallContext context)
        => await _mediator.Send(request, context.CancellationToken);

    private readonly IMediator _mediator;
    public TaskSchedulingService(IMediator mediator)
        => _mediator = mediator;
}
