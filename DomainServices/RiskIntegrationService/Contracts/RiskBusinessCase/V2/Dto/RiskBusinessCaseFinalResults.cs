using System.Text.Json.Serialization;

namespace DomainServices.RiskIntegrationService.Contracts.RiskBusinessCase.V2;

[ProtoContract]
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum RiskBusinessCaseFinalResults
{
#pragma warning disable CA1707 // Identifiers should not contain underscores
    [ProtoEnum]
    Unknown = 0,

    [ProtoEnum]
    CANCELLED_BY_CLIENT = 1,

    [ProtoEnum]
    REFUSED_BY_OFFICER = 2,

    [ProtoEnum]
    PROVIDED = 3,

    [ProtoEnum]
    ALREADY_PROVIDED_BY_EXT_SYS = 4,

    [ProtoEnum]
    TIMEOUT_BY_EXT_SYS = 5
#pragma warning restore CA1707 // Identifiers should not contain underscores
}
