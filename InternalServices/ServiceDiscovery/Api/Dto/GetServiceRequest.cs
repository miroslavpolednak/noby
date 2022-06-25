using CIS.Core.Types;
using CIS.InternalServices.ServiceDiscovery.Contracts;

namespace CIS.InternalServices.ServiceDiscovery.Api.Dto;

internal record GetServiceRequest : IRequest<GetServiceResponse> 
{
    public ApplicationEnvironmentName Environment { get; init; }
    public ServiceName ServiceName { get; init; }
    public ServiceTypes ServiceType { get; init; }

    public GetServiceRequest(
        ApplicationEnvironmentName environment,
        ServiceName serviceName,
        ServiceTypes serviceType
    )
    {
        if (serviceType == ServiceTypes.Unknown)
            throw new ArgumentOutOfRangeException(nameof(serviceType), "ServiceType must be specified in order to get single service detail");

        Environment = environment;
        ServiceName = serviceName;
        ServiceType = serviceType;
    }
}
