using System.Runtime.Serialization;

namespace CIS.InternalServices.ServiceDiscovery.Dto;

[DataContract]
[Serializable]
internal sealed class ServiceModel
{
    [DataMember(Order = 1)]
    public string ServiceName { get; set; } = "";

    [DataMember(Order = 2)]
    public Contracts.ServiceTypes ServiceType {  get; set; }

    [DataMember(Order = 3)]
    public string? ServiceUrl {  get; set; }
}
