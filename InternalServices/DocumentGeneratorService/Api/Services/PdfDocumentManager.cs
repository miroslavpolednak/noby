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

        //var xmp = new XmpMetadata();

        //xmp.AddSchema(new PdfASchema(PdfAStandard.PDF_A_1a_2005));

        //pdf.XmpMetadata = xmp;

        //var iccProfile = new Pdf.IccProfile("D:\\Users\\992589l\\Downloads\\AdobeICCProfilesCS4Win_end-user\\Adobe ICC Profiles (end-user)\\CMYK\\CoatedFOGRA27.icc");
        //var outputIntents = new Pdf.OutputIntent("", "CoatedFOGRA27", "https://www.adobe.com/", "CMYK", iccProfile);
        //outputIntents.Version = Pdf.OutputIntentVersion.PDF_A;

        //pdf.OutputIntents.Add(outputIntents);

        var path = Path.Combine("D:\\MPSS\\TestPdf", "NABIDKA_121022_result.pdf");

        pdf.Draw(path);

        return new Document();
    }

    private Pdf.Document ProcessParts(IEnumerable<GenerateDocumentPart> parts)
    {
        var documentStreams = new List<Stream>();

        foreach (var documentPart in parts)
        {
            var template = LoadPdfTemplate();

            new PdfAcroForm(new AcroFieldFormatProvider()).Fill(template, documentPart.Data);

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
        var path = Path.Combine("D:\\MPSS\\TestPdf", "NABIDKA_121022.pdf");

        var fileStream = File.OpenRead(path);

        return new MergeDocument(new PdfDocument(fileStream));
    }
}