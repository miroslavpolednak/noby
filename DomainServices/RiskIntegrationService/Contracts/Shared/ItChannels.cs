using System.Text.Json.Serialization;

namespace DomainServices.RiskIntegrationService.Contracts.Shared;

[ProtoContract]
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ItChannels
{
    [ProtoEnum]
    Unknown = 0,

    [ProtoEnum]
    NOBY = 1,

    [ProtoEnum]
    STARBUILD = 2,

    [ProtoEnum]
    DCS = 3,

    [ProtoEnum]
    PF = 4,

    [ProtoEnum]
    PFO = 5,

    [ProtoEnum]
    AON = 6
}
