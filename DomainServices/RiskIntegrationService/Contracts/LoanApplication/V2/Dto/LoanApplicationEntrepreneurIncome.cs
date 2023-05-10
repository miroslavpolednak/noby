namespace DomainServices.RiskIntegrationService.Contracts.LoanApplication.V2;

[ProtoContract]
public class LoanApplicationEntrepreneurIncome
{
    [ProtoMember(1)]
    public string? EntrepreneurIdentificationNumber { get; set; }

    [Obsolete("c4mv3")]
    [ProtoMember(2)]
    public int? ClassificationOfEconomicActivityId { get; set; }

    [Obsolete("c4mv3")]
    [ProtoMember(3)]
    public int? JobTypeId { get; set; }

    [ProtoMember(4)]
    public Shared.AddressDetail? Address { get; set; }

    [ProtoMember(5)]
    public DateTime? EstablishedOn { get; set; }

    [ProtoMember(6)]
    public bool IsDomicile { get; set; }

    [ProtoMember(7)]
    public int? ProofTypeId { get; set; }

    [ProtoMember(8)]
    public Shared.AmountDetail? AnnualIncomeAmount { get; set; }

    [ProtoMember(9)]
    public bool LumpSumTaxationRegime { get; set; }

    [ProtoMember(10)]
    public bool LumpSumModified { get; set; }
}
