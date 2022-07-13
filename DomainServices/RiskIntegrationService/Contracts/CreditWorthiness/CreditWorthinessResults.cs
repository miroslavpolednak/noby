using System.Text.Json.Serialization;

namespace DomainServices.RiskIntegrationService.Contracts.CreditWorthiness;

[ProtoContract]
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum CreditWorthinessResults
{
    [ProtoEnum]
    Unknown = 0,

    [ProtoEnum]
    Success = 1,

    [ProtoEnum]
    Failed = 2
}
