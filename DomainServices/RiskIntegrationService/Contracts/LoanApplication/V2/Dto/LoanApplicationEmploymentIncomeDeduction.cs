namespace DomainServices.RiskIntegrationService.Contracts.LoanApplication.V2;

[ProtoContract]
public class LoanApplicationEmploymentIncomeDeduction
{
    [ProtoMember(1)]
    public decimal? Execution { get; set; }

    [ProtoMember(2)]
    public decimal? Installments { get; set; }

    [ProtoMember(3)]
    public decimal? Other { get; set; }
}
