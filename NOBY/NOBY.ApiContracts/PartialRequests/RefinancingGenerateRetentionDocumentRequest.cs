namespace NOBY.ApiContracts;

public partial class RefinancingGenerateRetentionDocumentRequest
    : IRequest
{
    [JsonIgnore]
    public long CaseId { get; private set; }

    [JsonIgnore]
    public int SalesArrangementId { get; private set; }

    public RefinancingGenerateRetentionDocumentRequest Infuse(long caseId, int salesArrangementId)
    {
        CaseId = caseId;
        SalesArrangementId = salesArrangementId;
        return this;
    }
}
