using ceTe.DynamicPDF.Xmp;
using CIS.InternalServices.DocumentGeneratorService.Api.AcroForm;
using CIS.InternalServices.DocumentGeneratorService.Api.AcroForm.AcroFormWriter;
using CIS.InternalServices.DocumentGeneratorService.Api.Storage;
using Google.Protobuf;

namespace CIS.InternalServices.DocumentGeneratorService.Api.Services;

[TransientService, SelfService]
internal class PdfDocumentManager
{
    private readonly PdfAcroFormWriterFactory _pdfAcroFormWriterFactory;
    private readonly TemplateManager _templateManager;
    private readonly PdfFooter _pdfFooter;

    public PdfDocumentManager(PdfAcroFormWriterFactory pdfAcroFormWriterFactory, TemplateManager templateManager, PdfFooter pdfFooter)
    {
        _pdfAcroFormWriterFactory = pdfAcroFormWriterFactory;
        _templateManager = templateManager;
        _pdfFooter = pdfFooter;
    }

    public async Task<Contracts.Document> GenerateDocument(GenerateDocumentRequest request)
    {
        await ProcessParts(request.Parts);

        var finalPdf = await PrepareFinalPdf(request.OutputType, request);

        return new Contracts.Document
        {
            Data = await ConvertPdfToByteString(finalPdf)
        };
    }

    private async Task ProcessParts(IEnumerable<GenerateDocumentPart> parts)
    {
        foreach (var documentPart in parts)
        {
            var acroFormWriter = _pdfAcroFormWriterFactory.Create(documentPart.Data);

            var templateLoader = await _templateManager.CreateLoader(documentPart.DocumentTypeId, documentPart.DocumentTemplateVersion);

            var template = acroFormWriter.Write(templateLoader);
            
            _templateManager.DrawTemplate(template);
        }
    }

    private async Task<Document> PrepareFinalPdf(OutputFileType outputFileType, GenerateDocumentRequest request)
    {
        var finalDocument = await _templateManager.CreateFinalDocument(request.DocumentTypeId, request.DocumentTemplateVersion);

        await _pdfFooter.FillFooter(finalDocument, request);

        if (outputFileType is OutputFileType.Pdfa or OutputFileType.Unknown)
            ArchiveDocument(finalDocument.Document);

        return finalDocument.Document;
    }

    private static void ArchiveDocument(Document document)
    {
        var xmp = new XmpMetadata();

        xmp.AddSchema(new PdfASchema(PdfAStandard.PDF_A_1a_2005));

        xmp.DublinCore.Title.DefaultText = document.Title;
        xmp.DublinCore.Creators.Add(document.Author);
        xmp.DublinCore.Title.AddLang("cs-cz", "PDF/A1 Dokument");

        document.XmpMetadata = xmp;

        var iccProfile = new IccProfile("D:\\Users\\992589l\\Downloads\\AdobeICCProfilesCS4Win_end-user\\Adobe ICC Profiles (end-user)\\CMYK\\CoatedFOGRA27.icc");
        var outputIntents = new OutputIntent("", "CoatedFOGRA27", "https://www.adobe.com/", "CMYK", iccProfile)
        {
            Version = OutputIntentVersion.PDF_A
        };

        document.OutputIntents.Add(outputIntents);
    }

    private static Task<ByteString> ConvertPdfToByteString(Document document)
    {
        var memoryStream = new MemoryStream();

        document.Draw(memoryStream);

        memoryStream.Position = 0;

        return ByteString.FromStreamAsync(memoryStream);
    }
}