using System.Runtime.Serialization;

namespace CIS.Infrastructure.gRPC.CodeFirst;

[DataContract]
internal sealed class HealthCheckResponse
{
    [DataMember(Order = 1)]
    public ServingStatus status { get; set; }
}

internal enum ServingStatus
{
    UNKNOWN = 0,
    SERVING = 1,
#pragma warning disable CA1707 // Identifiers should not contain underscores
    NOT_SERVING = 2,
    SERVICE_UNKNOWN = 3
#pragma warning restore CA1707 // Identifiers should not contain underscores
}
