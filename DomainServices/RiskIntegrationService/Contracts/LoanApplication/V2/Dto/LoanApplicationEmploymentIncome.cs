namespace DomainServices.RiskIntegrationService.Contracts.LoanApplication.V2;

[ProtoContract]
public class LoanApplicationEmploymentIncome
{
    [ProtoMember(1)]
    public string? EmployerIdentificationNumber { get; set; }

    [ProtoMember(2)]
    public string EmployerName { get; set; } = default!;

    [ProtoMember(3)]
    public int? CountryId { get; set; }

    [ProtoMember(4)]
    public string? JobDescription { get; set; }

    [ProtoMember(5)]
    public Shared.AmountDetail? MonthlyIncomeAmount { get; set; }

    [ProtoMember(6)]
    public bool IsDomicile { get; set; }

    [ProtoMember(7)]
    public int? ProofTypeId { get; set; }

    [ProtoMember(8)]
    public string? ConfirmationPerson { get; set; }

    [ProtoMember(9)]
    public string? ConfirmationContactPhone { get; set; }

    [ProtoMember(10)]
    public DateTime? ConfirmationDate { get; set; }

    [ProtoMember(11)]
    public bool JobTrialPeriod { get; set; }

    [ProtoMember(12)]
    public bool NoticePeriod { get; set; }

    [ProtoMember(13)]
    public int? EmploymentTypeId { get; set; }

    [ProtoMember(14)]
    public DateTime? FirstWorkContractSince { get; set; }

    [ProtoMember(15)]
    public DateTime? CurrentWorkContractSince { get; set; }

    [ProtoMember(16)]
    public DateTime? CurrentWorkContractTo { get; set; }

    [ProtoMember(17)]
    public bool ConfirmationByCompany { get; set; }

    [ProtoMember(18)]
    public string? PhoneNumber { get; set; }

    [ProtoMember(19)]
    public Shared.BankAccountDetail? BankAccount { get; set; }

    [ProtoMember(20)]
    public int? IncomeForeignTypeId { get; set; }

    [ProtoMember(21)]
    public LoanApplicationEmploymentIncomeDeduction? IncomeDeduction { get; set; }
}
