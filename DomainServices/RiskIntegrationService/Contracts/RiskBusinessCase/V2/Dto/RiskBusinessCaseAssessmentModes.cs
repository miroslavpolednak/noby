using System.Text.Json.Serialization;

namespace DomainServices.RiskIntegrationService.Contracts.RiskBusinessCase.V2;

[ProtoContract]
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum RiskBusinessCaseAssessmentModes
{
    [ProtoEnum]
    Unknown = 0,

    [ProtoEnum]
    PS = 1,

    [ProtoEnum]
    SC = 2,

    [ProtoEnum]
    FI = 3
}
