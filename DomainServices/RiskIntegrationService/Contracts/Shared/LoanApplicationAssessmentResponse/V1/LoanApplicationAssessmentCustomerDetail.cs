
namespace DomainServices.RiskIntegrationService.Contracts.Shared.V1;

[ProtoContract]
public sealed class LoanApplicationAssessmentCustomerDetail
{
    [ProtoMember(1)]
    public long? InternalCustomerId { get; set; }

    [ProtoMember(2)]
    public string? PrimaryCustomerId { get; set; }

    [ProtoMember(3)]
    public LoanApplicationAssessmentDetail? Detail { get; set; }
}
