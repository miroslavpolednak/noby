namespace DomainServices.RiskIntegrationService.Contracts.RiskBusinessCase.V2;

[ProtoContract]
public class RiskBusinessCaseCreateRequest
    : IRequest<RiskBusinessCaseCreateResponse>, CIS.Core.Validation.IValidatableRequest
{
    [ProtoMember(1)]
    public long SalesArrangementId { get; set; }

    [ProtoMember(2)]
    public string? ResourceProcessId { get; set; }
}
