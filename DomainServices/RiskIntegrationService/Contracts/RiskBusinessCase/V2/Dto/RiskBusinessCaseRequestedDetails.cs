using System.Text.Json.Serialization;

namespace DomainServices.RiskIntegrationService.Contracts.RiskBusinessCase.V2;

[ProtoContract]
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum RiskBusinessCaseRequestedDetails
{
    [ProtoEnum]
    Unknown = 0,

    [ProtoEnum]
    assessmentDetail = 1,

    [ProtoEnum]
    householdAssessmentDetail = 2,

    [ProtoEnum]
    counterpartyAssessmentDetail = 3,

    [ProtoEnum]
    loanApplicationApprovalPossibilities = 4,

    [ProtoEnum]
    collateralRiskCharacteristics = 5
}
