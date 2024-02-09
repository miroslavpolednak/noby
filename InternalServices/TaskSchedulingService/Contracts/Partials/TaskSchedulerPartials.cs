using Google.Protobuf.WellKnownTypes;

namespace CIS.InternalServices.TaskSchedulingService.Contracts;

public partial class GetJobsRequest
    : MediatR.IRequest<GetJobsResponse>
{ }

public partial class GetTriggersRequest
    : MediatR.IRequest<GetTriggersResponse>
{ }

public partial class GetInstanceStatusRequest
    : MediatR.IRequest<GetInstanceStatusResponse>
{ }

public partial class ExecuteJobRequest
    : MediatR.IRequest<ExecuteJobResponse>
{ }

public partial class UpdateSchedulerRequest
    : MediatR.IRequest<Empty>
{ }