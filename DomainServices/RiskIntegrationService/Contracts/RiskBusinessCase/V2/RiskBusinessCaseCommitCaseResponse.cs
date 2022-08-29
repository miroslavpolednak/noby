namespace DomainServices.RiskIntegrationService.Contracts.RiskBusinessCase.V2;

[ProtoContract, CompatibilityLevel(CompatibilityLevel.Level300)]
public class RiskBusinessCaseCommitCaseResponse
{
    [DataMember(Order = 1)]
    public string RiskBusinessCaseId { get; set; } = default!;

    [DataMember(Order = 2)]
    public DateTime Timestamp { get; set; }

    [DataMember(Order = 3)]
    public string OperationResult { get; set; } = default!;

    [DataMember(Order = 4)]
    public string? FinalState { get; set; }

    [DataMember(Order = 5)]
    public List<Shared.ResultReasonDetail>? ResultReasons { get; set; }
}