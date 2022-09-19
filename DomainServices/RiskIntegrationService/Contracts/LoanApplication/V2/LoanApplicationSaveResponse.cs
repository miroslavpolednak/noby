namespace DomainServices.RiskIntegrationService.Contracts.LoanApplication.V2;

[ProtoContract]
public class LoanApplicationSaveResponse
{
    [ProtoMember(1)]
    public LoanApplicationRiskSegments RiskSegment { get; set; }
}
