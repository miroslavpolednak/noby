namespace CIS.InternalServices.DocumentGeneratorService.Api.Storage;

public class FinalDocument
{
    public FinalDocument(Document document)
    {
        Document = document;
    }

    public Document Document { get; }

    public IList<PdfDocument> PdfDocumentParts { get; } = new List<PdfDocument>();
}