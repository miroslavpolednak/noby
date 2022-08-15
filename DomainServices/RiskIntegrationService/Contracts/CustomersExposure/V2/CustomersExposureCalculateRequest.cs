namespace DomainServices.RiskIntegrationService.Contracts.CustomersExposure.V2;

[ProtoContract]
public class CustomersExposureCalculateRequest
    : IRequest<CustomersExposureCalculateResponse>, CIS.Core.Validation.IValidatableRequest
{
    [ProtoMember(1)]
    public long CaseId { get; set; }

    [ProtoMember(2)]
    public string RiskBusinessCaseId { get; set; } = null!;

    [ProtoMember(3)]
    public string LoanApplicationDataVersion { get; set; } = null!;

    [ProtoMember(4)]
    public Shared.Identity UserIdentity { get; set; } = null!;
}
