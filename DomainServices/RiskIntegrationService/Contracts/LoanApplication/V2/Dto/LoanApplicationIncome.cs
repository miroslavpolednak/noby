namespace DomainServices.RiskIntegrationService.Contracts.LoanApplication.V2;

[ProtoContract, CompatibilityLevel(CompatibilityLevel.Level300)]
public class LoanApplicationIncome
{
    [ProtoMember(1)]
    public bool IsIncomeConfirmed { get; set; }

    [ProtoMember(2)]
    public DateTime? LastConfirmedDate { get; set; }    
}
