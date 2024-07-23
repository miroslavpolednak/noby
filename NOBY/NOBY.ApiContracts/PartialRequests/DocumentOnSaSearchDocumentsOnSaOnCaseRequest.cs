namespace NOBY.ApiContracts;

public partial class DocumentOnSaSearchDocumentsOnSaOnCaseRequest : IRequest<DocumentOnSaSearchDocumentsOnSaOnCaseResponse>
{
    [JsonIgnore]
    public long CaseId { get; set; }

    public DocumentOnSaSearchDocumentsOnSaOnCaseRequest InfuseCaseId(long caseId)
    {
        CaseId = caseId;
        return this;
    }
}
