
namespace DomainServices.RiskIntegrationService.Contracts.Shared.V1;

[ProtoContract]
public sealed class LoanApplicationAssessmentHouseholdDetail
{
    [ProtoMember(1)]
    public long? HouseholdId { get; set; }

    [ProtoMember(2)]
    public LoanApplicationAssessmentDetail? Detail { get; set; }
}
