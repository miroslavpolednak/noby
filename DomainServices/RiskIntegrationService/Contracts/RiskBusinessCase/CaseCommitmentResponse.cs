namespace DomainServices.RiskIntegrationService.Contracts.RiskBusinessCase;

[DataContract]
public class CaseCommitmentResponse
{
    /// <summary>
    /// Datum a čas odpovědi
    /// </summary>
    /// <value>Datum a čas odpovědi</value>
    [DataMember(Order = 1)]
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// Status
    /// </summary>
    /// <value>Status</value>
    [DataMember(Order = 2)]
    public string OperationResult { get; set; } = default!;

    /// <summary>
    /// Důvod(y) nespočetní výsledků bonity
    /// </summary>
    /// <value>Důvod(y) nespočetní výsledků bonity</value>
    [DataMember(Order = 3)]
    public List<CommitmentResultReason>? ResultReasons { get; set; }
}
