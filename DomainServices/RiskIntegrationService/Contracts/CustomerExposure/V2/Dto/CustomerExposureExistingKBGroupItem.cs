using DomainServices.RiskIntegrationService.Contracts.Shared;

namespace DomainServices.RiskIntegrationService.Contracts.CustomerExposure.V2;

[ProtoContract]
public class CustomerExposureExistingKBGroupItem
{
    [ProtoMember(1)]
    public BankAccountDetail? BankAccount { get; set; }

    [ProtoMember(2)]
    public int? LoanType { get; set; }

    [ProtoMember(3)]
    public int? LoanTypeCategory { get; set; }

    [ProtoMember(4)]
    public int? CustomerRoleId { get; set; }

    [ProtoMember(5)]
    public decimal? LoanAmount { get; set; }

    [ProtoMember(6)]
    public decimal? DrawingAmount { get; set; }

    [ProtoMember(7)]
    public DateTime? ContractDate { get; set; }

    [ProtoMember(8)]
    public DateTime? MaturityDate { get; set; }

    [ProtoMember(9)]
    public decimal? LoanBalanceAmount { get; set; }

    [ProtoMember(10)]
    public decimal? LoanOnBalanceAmount { get; set; }

    [ProtoMember(11)]
    public decimal? LoanOffBalanceAmount { get; set; }

    [ProtoMember(12)]
    public decimal? ExposureAmount { get; set; }

    [ProtoMember(13)]
    public decimal? InstallmentAmount { get; set; }

    [ProtoMember(14)]
    public bool IsSecured { get; set; }
}
