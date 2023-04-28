namespace NOBY.Api.Endpoints.Cases.GetCaseDocumentsFlag;

public class GetCaseDocumentsFlagResponse
{
    public DocumentsMenuItem DocumentsMenuItem { get; set; } = null!;
}

public class DocumentsMenuItem
{
    /// <summary>
    /// 0 - No flag, 1 - Error (!), 2 - In processing
    /// </summary>
    public FlagDocuments Flag { get; set; }
}

public enum FlagDocuments
{
    NoFlag = 0,
    Error = 1,
    InProcessing = 2

}