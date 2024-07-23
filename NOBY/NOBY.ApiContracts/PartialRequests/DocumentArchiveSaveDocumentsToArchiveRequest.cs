namespace NOBY.ApiContracts;

public partial class DocumentArchiveSaveDocumentsToArchiveRequest : IRequest
{
    [JsonIgnore]
    public long CaseId;

    public DocumentArchiveSaveDocumentsToArchiveRequest InfuseId(long caseId)
    {
        CaseId = caseId;
        return this;
    }
}
