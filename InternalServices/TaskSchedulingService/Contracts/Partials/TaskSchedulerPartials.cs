using Google.Protobuf.WellKnownTypes;

namespace CIS.InternalServices.TaskSchedulingService.Contracts;

public partial class GetJobsRequest
    : MediatR.IRequest<GetJobsResponse>
{ }

public partial class GetTriggersRequest
    : MediatR.IRequest<GetTriggersResponse>
{ }
