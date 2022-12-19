namespace NOBY.Api.Endpoints.Document.GetDocument;

internal class GetDocumentResponse
{
    public required long? CaseId { get; init; }

    public required byte[] Buffer { get; init; }
}