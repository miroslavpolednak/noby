using CIS.InternalServices.TaskSchedulingService.Contracts;

namespace CIS.InternalServices.TaskSchedulingService.Api.Endpoints.GetAvailableTasks;

internal sealed class GetAvailableTasksHandler
    : IRequestHandler<GetAvailableTasksRequest, GetAvailableTasksResponse>
{
    public async Task<GetAvailableTasksResponse> Handle(GetAvailableTasksRequest request, CancellationToken cancellation)
    {
        return new GetAvailableTasksResponse();
    }
}
