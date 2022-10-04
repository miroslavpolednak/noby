using ceTe.DynamicPDF.Forms;
using ceTe.DynamicPDF.Merger;
using CIS.Infrastructure.Attributes;
using CIS.InternalServices.DocumentGeneratorService.Api.AcroForm;
using CIS.InternalServices.DocumentGeneratorService.Contracts;
using Pdf = ceTe.DynamicPDF;

namespace CIS.InternalServices.DocumentGeneratorService.Api.Services;

[ScopedService, SelfService]
public class PdfDocumentManager
{
    public PdfDocumentManager()
    {
    }

    public Document GenerateDocument(GenerateDocumentRequest request)
    {
        var pdf = ProcessParts(request.Parts);

        var path = Path.Combine("D:\\Users\\992589l\\Downloads", "Nabídka_KB_EDIT_2_out.pdf");

        var path2 = Path.Combine("D:\\Users\\992589l\\Downloads", "Nabídka_KB_EDIT_2.pdf");
        var test = new ImportedPageArea(path2, 2, 0, 0);

        pdf.Pages[0].Elements.Add(test);

        pdf.Draw(path);

        return new Document();
    }

    private Pdf.Document ProcessParts(IEnumerable<GenerateDocumentPart> parts)
    {
        var documentStreams = new List<Stream>();

        foreach (var documentPart in parts)
        {
            var template = LoadPdfTemplate();

            new PdfAcroForm().Fill(template, documentPart.Data);

            documentStreams.Add(new MemoryStream());

            template.Draw(documentStreams.Last());
        }

        var document = new MergeDocument
        {
            Form = { Output = FormOutput.Flatten }
        };

        documentStreams.ForEach(stream => document.Append(new PdfDocument(stream)));

        return document;
    }

    private Pdf.Document LoadPdfTemplate()
    {
        var path = Path.Combine("D:\\Users\\992589l\\Downloads", "Nabídka_KB_EDIT_2.pdf");

        var fileStream = File.OpenRead(path);

        return new MergeDocument(new PdfDocument(fileStream));
    }
}