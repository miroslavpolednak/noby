namespace DomainServices.RiskIntegrationService.Contracts.CustomerExposure.V2;

[ProtoContract]
public class CustomerExposureCalculateRequest
    : IRequest<CustomerExposureCalculateResponse>, CIS.Core.Validation.IValidatableRequest
{
    [ProtoMember(1)]
    public long SalesArrangementId { get; set; }

    [ProtoMember(2)]
    public string RiskBusinessCaseId { get; set; } = null!;

    [ProtoMember(3)]
    public string LoanApplicationDataVersion { get; set; } = null!;

    [ProtoMember(4)]
    public Shared.Identity? UserIdentity { get; set; } = null!;
}
