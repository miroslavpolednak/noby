using CIS.Infrastructure.ExternalServicesHelpers;

namespace ExternalServices.SbQueues.Configuration;

public class SbQueuesConfiguration
{
    public ServiceImplementationTypes ImplementationType { get; set; } = ServiceImplementationTypes.Unknown;
}