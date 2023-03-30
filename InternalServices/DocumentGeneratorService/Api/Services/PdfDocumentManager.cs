using ceTe.DynamicPDF.PageElements;
using ceTe.DynamicPDF.Xmp;
using CIS.InternalServices.DocumentGeneratorService.Api.AcroForm;
using CIS.InternalServices.DocumentGeneratorService.Api.AcroForm.AcroFormWriter;
using CIS.InternalServices.DocumentGeneratorService.Api.Storage;
using Google.Protobuf;
using Path = System.IO.Path;

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
            var template = await _templateManager.LoadTemplate(documentPart.DocumentTypeId, documentPart.DocumentTemplateVersionId, documentPart.DocumentTemplateVariantId);

            var document = _pdfAcroFormWriterFactory.Create(documentPart.Data).Write(template);
            
            _templateManager.DrawTemplate(document);
        }
    }

    private async Task<Document> PrepareFinalPdf(OutputFileType outputFileType, GenerateDocumentRequest request)
    {
        var finalDocument = await _templateManager.CreateFinalDocument(request.DocumentTypeId, request.DocumentTemplateVersionId, request.DocumentTemplateVariantId);

        await _pdfFooter.FillFooter(finalDocument, request);

        if (request.ForPreview ?? true)
            AddWatermark(finalDocument.Document);

        if (outputFileType is OutputFileType.Pdfa or OutputFileType.Unknown)
            ArchiveDocument(finalDocument.Document);

        return finalDocument.Document;
    }

    private void ArchiveDocument(Document document)
    {
        var xmp = new XmpMetadata();

        xmp.AddSchema(new PdfASchema(PdfAStandard.PdfA2a));

        xmp.DublinCore.Title.DefaultText = document.Title;
        xmp.DublinCore.Creators.Add(document.Author);
        xmp.DublinCore.Title.AddLang("cs-cz", "PDF/A1 Dokument");

        document.XmpMetadata = xmp;
        
        var iccProfile = new IccProfile(Path.Combine(_templateManager.StoragePath, "ICC\\CoatedFOGRA27.icc"));
        var outputIntents = new OutputIntent("", "CoatedFOGRA27", "https://www.adobe.com/", "CMYK", iccProfile)
        {
            Version = OutputIntentVersion.PDF_A
        };

        document.OutputIntents.Add(outputIntents);
    }

    private static void AddWatermark(Document document)
    {
        var textWatermark = new TextWatermark("Pouze pro informaci", Font.LoadSystemFont("Arial"), 68)
        {
            TextColor = new RgbColor(192, 192, 192),
            Angle = -45,
            Position = WatermarkPosition.Center
        };

        document.Template ??= new Template();

        document.Template.Elements.Add(textWatermark);
    }

    private static Task<ByteString> ConvertPdfToByteString(Document document)
    {
        var memoryStream = new MemoryStream();

        document.Draw(memoryStream);

        memoryStream.Position = 0;

        return ByteString.FromStreamAsync(memoryStream);
    }
}