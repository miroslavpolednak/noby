namespace CIS.InternalServices.DocumentGeneratorService.Api.AcroForm.AcroFormWriter;

public interface IAcroFormWriter
{
    MergeDocument Write(PdfDocument pdfDocument, string? templateNameModifier = default);
}