namespace DomainServices.RiskIntegrationService.Contracts.CustomersExposure.V1;

[ProtoContract]
public class CustomersExposureRequestedKBGroupItem
{
    [ProtoMember(1)]
    public string? RiskBusinessCaseId { get; set; }

    [ProtoMember(2)]
    public string? LoanType { get; set; }

    [ProtoMember(3)]
    public string? LoanTypeCategory { get; set; }

    [ProtoMember(4)]
    public int? CustomerRoleId { get; set; }

    [ProtoMember(5)]
    public decimal? LoanAmount { get; set; }

    [ProtoMember(6)]
    public decimal? InstallmentAmount { get; set; }

    [ProtoMember(7)]
    public bool IsSecured { get; set; }

    [ProtoMember(8)]
    public string? StatusCode { get; set; }
}
