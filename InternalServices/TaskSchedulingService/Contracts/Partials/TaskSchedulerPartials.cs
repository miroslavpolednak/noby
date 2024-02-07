using Google.Protobuf.WellKnownTypes;

namespace CIS.InternalServices.TaskSchedulingService.Contracts;

public partial class GetAvailableTasksRequest
    : MediatR.IRequest<GetAvailableTasksResponse>
{ }
