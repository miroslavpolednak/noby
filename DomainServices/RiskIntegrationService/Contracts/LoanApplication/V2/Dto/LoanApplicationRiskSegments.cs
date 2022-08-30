using System.Text.Json.Serialization;

namespace DomainServices.RiskIntegrationService.Contracts.LoanApplication.V2;

[ProtoContract]
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum LoanApplicationRiskSegments
{
    [ProtoEnum]
    Unknown = 0,

    [ProtoEnum]
    A = 1,

    [ProtoEnum]
    B = 2
}
