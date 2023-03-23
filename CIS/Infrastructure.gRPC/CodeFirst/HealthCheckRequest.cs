using System.Runtime.Serialization;

namespace CIS.Infrastructure.gRPC.CodeFirst;

[DataContract]
internal sealed class HealthCheckRequest
{
    [DataMember(Order = 1)]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public string service { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}
