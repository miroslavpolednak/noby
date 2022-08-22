namespace DomainServices.RiskIntegrationService.Contracts.LoanApplication.V2;

[ProtoContract, CompatibilityLevel(CompatibilityLevel.Level300)]
public sealed class LoanApplicationRentIncome
{
    [ProtoMember(1)]
    public Shared.BankAccountDetail? BankAccount { get; set; }

    [ProtoMember(2)]
    public bool IsDomicile { get; set; }

    [ProtoMember(3)]
    public int? ProofTypeId { get; set; }

    [ProtoMember(4)]
    public Shared.AmountDetail? MonthlyAmount { get; set; }
}
