namespace DomainServices.RiskIntegrationService.Contracts.LoanApplication.V2;

[ProtoContract]
public class LoanApplicationEntrepreneurIncome
{
    [ProtoMember(1)]
    public string? EntrepreneurIdentificationNumber { get; set; }

    [ProtoMember(2)]
    public int? CountryId { get; set; }

    [ProtoMember(3)]
    public DateTime? EstablishedOn { get; set; }

    [ProtoMember(4)]
    public bool IsDomicile { get; set; }

    [ProtoMember(5)]
    public int? ProofTypeId { get; set; }

    [ProtoMember(6)]
    public Shared.AmountDetail? AnnualIncomeAmount { get; set; }

    [ProtoMember(7)]
    public bool LumpSumTaxationRegime { get; set; }

    [ProtoMember(8)]
    public bool LumpSumModified { get; set; }
}
