using ceTe.DynamicPDF.Xmp;
using CIS.InternalServices.DocumentGeneratorService.Api.AcroForm;
using CIS.InternalServices.DocumentGeneratorService.Api.Storage;
using CIS.InternalServices.DocumentGeneratorService.Contracts;
using Google.Protobuf;

namespace CIS.InternalServices.DocumentGeneratorService.Api.Services;

[TransientService, SelfService]
internal class PdfDocumentManager
{
    private readonly TemplateManager _templateManager;
    private readonly PdfAcroForm _pdfAcroForm;
    private readonly PdfFooter _pdfFooter;

    public PdfDocumentManager(TemplateManager templateManager, PdfAcroForm pdfAcroForm, PdfFooter pdfFooter)
    {
        _templateManager = templateManager;
        _pdfAcroForm = pdfAcroForm;
        _pdfFooter = pdfFooter;
    }

    public async Task<Document> GenerateDocument(GenerateDocumentRequest request)
    {
        await ProcessParts(request.Parts);

        var finalPdf = await PrepareFinalPdf(request.OutputType, request.DocumentFooter);

        return new Document
        {
            Data = await ConvertPdfToByteString(finalPdf)
        };
    }

    private async Task ProcessParts(IEnumerable<GenerateDocumentPart> parts)
    {
        foreach (var documentPart in parts)
        {
            var template = await _templateManager.LoadTemplate(documentPart.TemplateTypeId, documentPart.TemplateVersion);

            _pdfAcroForm.Fill(template, documentPart.Data);

            _templateManager.DrawTemplate(template);
        }
    }

    private async Task<Pdf.Document> PrepareFinalPdf(OutputFileType outputFileType, DocumentFooter footer)
    {
        var finalPdf = _templateManager.CreateFinalDocument();

        await _pdfFooter.FillFooter(finalPdf, footer);

        if (outputFileType is OutputFileType.Pdfa or OutputFileType.Unknown)
            ArchiveDocument(finalPdf);

        return finalPdf;
    }

    private static void ArchiveDocument(Pdf.Document document)
    {
        var xmp = new XmpMetadata();

        xmp.AddSchema(new PdfASchema(PdfAStandard.PDF_A_1a_2005));

        xmp.DublinCore.Title.DefaultText = document.Title;
        xmp.DublinCore.Creators.Add(document.Author);
        xmp.DublinCore.Title.AddLang("cs-cz", "PDF/A1 Dokument");

        document.XmpMetadata = xmp;

        var iccProfile = new Pdf.IccProfile("D:\\Users\\992589l\\Downloads\\AdobeICCProfilesCS4Win_end-user\\Adobe ICC Profiles (end-user)\\CMYK\\CoatedFOGRA27.icc");
        var outputIntents = new Pdf.OutputIntent("", "CoatedFOGRA27", "https://www.adobe.com/", "CMYK", iccProfile)
        {
            Version = Pdf.OutputIntentVersion.PDF_A
        };

        document.OutputIntents.Add(outputIntents);
    }

    private static Task<ByteString> ConvertPdfToByteString(Pdf.Document document)
    {
        var memoryStream = new MemoryStream();

        document.Draw(memoryStream);

        memoryStream.Position = 0;

        return ByteString.FromStreamAsync(memoryStream);
    }
}