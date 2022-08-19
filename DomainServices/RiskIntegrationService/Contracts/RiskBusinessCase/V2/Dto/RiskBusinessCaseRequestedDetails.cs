using System.Text.Json.Serialization;

namespace DomainServices.RiskIntegrationService.Contracts.RiskBusinessCase.V2;

[ProtoContract]
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum RiskBusinessCaseRequestedDetails
{
    [ProtoEnum]
    Unknown = 0,

    [ProtoEnum]
    AssessmentDetail = 1,

    [ProtoEnum]
    HouseholdAssessmentDetail = 2,

    [ProtoEnum]
    CounterpartyAssessmentDetail = 3,

    [ProtoEnum]
    LoanApplicationApprovalPossibilities = 4,

    [ProtoEnum]
    CollateralRiskCharacteristics = 5
}
