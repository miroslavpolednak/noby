namespace CIS.InternalServices.ServiceDiscovery.Contracts;

public partial class GetServiceRequest
    : MediatR.IRequest<GetServiceResponse>
{ }

public partial class GetServicesRequest
    : MediatR.IRequest<GetServicesResponse>
{ }

public partial class ClearCacheRequest
    : MediatR.IRequest<Google.Protobuf.WellKnownTypes.Empty>
{ }