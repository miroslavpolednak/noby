
namespace DomainServices.RiskIntegrationService.Contracts.Shared.V1;

[ProtoContract]
public sealed class LoanApplicationAssessmentScore
{
    [ProtoMember(1)]
    public string? Value { get; set; }

    [ProtoMember(2)]
    public string? Scale { get; set; }
}
