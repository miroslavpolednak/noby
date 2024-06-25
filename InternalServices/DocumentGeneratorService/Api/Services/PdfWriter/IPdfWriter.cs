using CIS.InternalServices.DocumentGeneratorService.Api.Model.PdfMap;

namespace CIS.InternalServices.DocumentGeneratorService.Api.Services.PdfWriter;

public interface IPdfWriter
{

    MergeDocument WriteToDocument(PdfDocument pdfDocument, IReadOnlyDictionary<string, DocumentMapItem> fieldMap, ICollection<GenerateDocumentPartData> values);

}