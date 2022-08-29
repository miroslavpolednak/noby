using System.Text.Json.Serialization;

namespace DomainServices.RiskIntegrationService.Contracts.RiskBusinessCase.V2;

[ProtoContract]
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum RiskBusinessCaseGrantingProcedureCodes
{
    [ProtoEnum]
    Unknown = 0,

    [ProtoEnum]
    CPG = 1,

    [ProtoEnum]
    SC = 2,

    [ProtoEnum]
    EMP = 3,

    [ProtoEnum]
    NCR = 4,

    [ProtoEnum]
    NSR = 5,

    [ProtoEnum]
    QLC = 6,

    [ProtoEnum]
    SEL = 7,

    [ProtoEnum]
    STD = 8,

    [ProtoEnum]
    STU = 9,

    [ProtoEnum]
    XM = 10,

    [ProtoEnum]
    XT = 11
}
