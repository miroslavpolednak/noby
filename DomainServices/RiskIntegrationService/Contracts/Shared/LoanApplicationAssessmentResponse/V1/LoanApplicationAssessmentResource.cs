namespace DomainServices.RiskIntegrationService.Contracts.Shared.V1;

[ProtoContract]
public sealed class LoanApplicationAssessmentResource
{
    [ProtoMember(1)]
    public string? Entity { get; set; }

    [ProtoMember(2)]
    public ResourceIdentifier? Identifier { get; set; }
}
