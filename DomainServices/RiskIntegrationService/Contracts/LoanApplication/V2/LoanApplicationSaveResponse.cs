namespace DomainServices.RiskIntegrationService.Contracts.LoanApplication.V2;

[ProtoContract]
public class LoanApplicationSaveResponse
{
    public LoanApplicationRiskSegments RiskSegment { get; set; }

    public string LoanApplicationDataVersion { get; set; } = null!;

    public string LoanApplicationId { get; set; } = null!; 
}
