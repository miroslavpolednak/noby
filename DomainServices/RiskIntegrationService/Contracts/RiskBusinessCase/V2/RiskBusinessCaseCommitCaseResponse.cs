namespace DomainServices.RiskIntegrationService.Contracts.RiskBusinessCase.V2;

[ProtoContract, CompatibilityLevel(CompatibilityLevel.Level300)]
public class RiskBusinessCaseCommitCaseResponse
{
    [ProtoMember(1)]
    public string RiskBusinessCaseId { get; set; } = default!;

    [ProtoMember(2)]
    public DateTime Timestamp { get; set; }

    [ProtoMember(3)]
    public string OperationResult { get; set; } = default!;

    [ProtoMember(4)]
    public string? FinalState { get; set; }

    [ProtoMember(5)]
    public List<Shared.ResultReasonDetail>? ResultReasons { get; set; }
}