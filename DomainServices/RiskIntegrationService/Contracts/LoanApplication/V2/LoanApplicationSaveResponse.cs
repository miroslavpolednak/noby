namespace DomainServices.RiskIntegrationService.Contracts.LoanApplication.V2;

[ProtoContract]
public class LoanApplicationSaveResponse
{
    public string? RiskSegment { get; set; }
}
