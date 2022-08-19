namespace DomainServices.RiskIntegrationService.Contracts.LoanApplication.V2;

[ProtoContract, CompatibilityLevel(CompatibilityLevel.Level300)]
public class LoanApplicationIncome
{
    [ProtoMember(1)]
    public bool IsIncomeConfirmed { get; set; }

    [ProtoMember(2)]
    public DateTime LastConfirmedDate { get; set; }

    [ProtoMember(3)]
    public List<LoanApplicationEmploymentIncome>? EmploymentIncome { get; set; }

    [ProtoMember(4)]
    public List<LoanApplicationEntrepreneurIncome>? EntrepreneurIncome { get; set; }

    [ProtoMember(5)]
    public List<LoanApplicationRentIncome>? RentIncome { get; set; }

    [ProtoMember(6)]
    public List<LoanApplicationOtherIncome>? OtherIncome { get; set; }
}