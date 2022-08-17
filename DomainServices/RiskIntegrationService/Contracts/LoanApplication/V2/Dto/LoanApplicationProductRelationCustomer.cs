namespace DomainServices.RiskIntegrationService.Contracts.LoanApplication.V2;

[ProtoContract]
public class LoanApplicationProductRelationCustomer
{
    [ProtoMember(1)]
    public string CustomerId { get; set; } = null!;

    [ProtoMember(2)]
    public int CustomerRoleId { get; set; }
}
