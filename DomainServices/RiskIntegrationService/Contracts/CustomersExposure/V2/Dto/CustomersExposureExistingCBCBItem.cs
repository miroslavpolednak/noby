namespace DomainServices.RiskIntegrationService.Contracts.CustomersExposure.V2;

[ProtoContract, CompatibilityLevel(CompatibilityLevel.Level300)]
public class CustomersExposureExistingCBCBItem
{
    [ProtoMember(1)]
    public string? CbcbContractId { get; set; }

    [ProtoMember(2)]
    public int? CustomerRoleId { get; set; }

    [ProtoMember(3)]
    public string? LoanType { get; set; }

    [ProtoMember(4)]
    public string? LoanTypeCategory { get; set; }

    [ProtoMember(5)]
    public DateTime? MaturityDate { get; set; }

    [ProtoMember(6)]
    public decimal? LoanAmount { get; set; }

    [ProtoMember(7)]
    public decimal? InstallmentAmount { get; set; }

    [ProtoMember(8)]
    public decimal? ExposureAmount { get; set; }

    [ProtoMember(9)]
    public DateTime? ContractDate { get; set; }

    [ProtoMember(10)]
    public string? KbGroupInstanceCode { get; set; }

    [ProtoMember(11)]
    public DateTime? CbcbDataLastUpdate { get; set; }
}
