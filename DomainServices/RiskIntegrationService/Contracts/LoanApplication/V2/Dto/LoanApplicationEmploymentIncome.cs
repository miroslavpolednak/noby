namespace DomainServices.RiskIntegrationService.Contracts.LoanApplication.V2;

[ProtoContract, CompatibilityLevel(CompatibilityLevel.Level300)]
public class LoanApplicationEmploymentIncome
{
    [ProtoMember(1)]
    public string? EmployerIdentificationNumber { get; set; }

    [ProtoMember(2)]
    public int? WorkSectorId { get; set; }

    [ProtoMember(3)]
    public string? EmployerName { get; set; }

    [ProtoMember(4)]
    public int? ClassficationOfEconomicActivityId { get; set; }

    [ProtoMember(5)]
    public int? JobTypeId { get; set; }

    [ProtoMember(6)]
    public Shared.AddressDetail? Address { get; set; }

    [ProtoMember(7)]
    public string? JobDescription { get; set; }

    [ProtoMember(8)]
    public decimal? MonthlyIncomeAmount { get; set; }

    [ProtoMember(9)]
    public bool IsDomicile { get; set; }

    [ProtoMember(10)]
    public int? ProofTypeId { get; set; }

    [ProtoMember(11)]
    public decimal? GrossAnnualIncome { get; set; }

    [ProtoMember(12)]
    public string? ConfirmationPerson { get; set; }

    [ProtoMember(13)]
    public string? ConfirmationContactPhone { get; set; }

    [ProtoMember(14)]
    public DateTime? ConfirmationDate { get; set; }

    [ProtoMember(15)]
    public bool JobTrialPeriod { get; set; }

    [ProtoMember(16)]
    public bool NoticePeriod { get; set; }

    [ProtoMember(17)]
    public int? EmploymentTypeId { get; set; }

    [ProtoMember(18)]
    public DateTime? FirstWorkContractSince { get; set; }

    [ProtoMember(19)]
    public DateTime? CurrentWorkContractSince { get; set; }

    [ProtoMember(20)]
    public DateTime? CurrentWorkContractTo { get; set; }

    [ProtoMember(21)]
    public bool ConfirmationByCompany { get; set; }

    [ProtoMember(22)]
    public string? PhoneNumber { get; set; }

    [ProtoMember(23)]
    public Shared.BankAccountDetail? BankAccount { get; set; }

    [ProtoMember(24)]
    public int? IncomeForeignTypeId { get; set; }
}
