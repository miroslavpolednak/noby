using ceTe.DynamicPDF.PageElements;
using ceTe.DynamicPDF.Xmp;
using CIS.InternalServices.DocumentGeneratorService.Api.AcroForm;
using CIS.InternalServices.DocumentGeneratorService.Api.AcroForm.AcroFormWriter;
using CIS.InternalServices.DocumentGeneratorService.Api.Storage;
using Google.Protobuf;
using Microsoft.FeatureManagement;
using SharedTypes;

namespace CIS.InternalServices.DocumentGeneratorService.Api.Services;

[TransientService, SelfService]
internal class PdfDocumentManager
{
    private readonly PdfAcroFormWriterFactory _pdfAcroFormWriterFactory;
    private readonly TemplateManager _templateManager;
    private readonly PdfFooter _pdfFooter;
    private readonly PdfFieldMap _pdfFieldMap;
    private readonly IFeatureManager _featureManager;

    public PdfDocumentManager(PdfAcroFormWriterFactory pdfAcroFormWriterFactory, TemplateManager templateManager, PdfFooter pdfFooter, PdfFieldMap pdfFieldMap, IFeatureManager featureManager)
    {
        _pdfAcroFormWriterFactory = pdfAcroFormWriterFactory;
        _templateManager = templateManager;
        _pdfFooter = pdfFooter;
        _pdfFieldMap = pdfFieldMap;
        _featureManager = featureManager;
    }

    public async Task<Contracts.Document> GenerateDocument(GenerateDocumentRequest request)
    {
        await ProcessParts(request);

        var finalPdf = await PrepareFinalPdf(request.OutputType, request);

        return new Contracts.Document
        {
            Data = await ConvertPdfToByteString(finalPdf)
        };
    }

    private async Task ProcessParts(GenerateDocumentRequest request)
    {
        foreach (var documentPart in request.Parts)
        {
            var template = await _templateManager.LoadTemplate(documentPart.DocumentTypeId, documentPart.DocumentTemplateVersionId, documentPart.DocumentTemplateVariantId);

            MergeDocument document;
            if (await _featureManager.IsEnabledAsync(FeatureFlagsConstants.UseFieldsMap))
            {
                var pdfFieldMap = _pdfFieldMap.GetPdfMap(template.Name, template.Version, template.Variant);

                document = _pdfAcroFormWriterFactory.CreateWriter(documentPart.Data).WriteToDocument(template.PdfDocument, pdfFieldMap, documentPart.Data);

                new PdfElements.PdfFooter().FillFooter(template, document, pdfFieldMap, request);
            }
            else
            {
                document = _pdfAcroFormWriterFactory.Create(documentPart.Data).Write(template.PdfDocument);
            }

            _templateManager.DrawTemplate(document);
        }
    }

    private async Task<Document> PrepareFinalPdf(OutputFileType outputFileType, GenerateDocumentRequest request)
    {
        var finalDocument = await _templateManager.CreateFinalDocument(request.DocumentTypeId, request.DocumentTemplateVersionId, request.DocumentTemplateVariantId);

        if (await _featureManager.IsEnabledAsync(FeatureFlagsConstants.UseFieldsMap))
        {
            finalDocument.Document.Form.Output = FormOutput.Remove;
        }
        else
        {
            await _pdfFooter.FillFooter(finalDocument, request);
        }

        if (request.ForPreview ?? true)
            AddWatermark(finalDocument.Document);

        if (outputFileType is OutputFileType.Pdfa or OutputFileType.Unknown)
            ArchiveDocument(finalDocument.Document);

        return finalDocument.Document;
    }

    private static void ArchiveDocument(Document document)
    {
        var xmp = new XmpMetadata();
        xmp.AddSchema(new PdfASchema(PdfAStandard.PdfA2b));

        xmp.DublinCore.Title.DefaultText = document.Title;
        xmp.DublinCore.Creators.Add(document.Author);
        xmp.DublinCore.Title.AddLang("cs-cz", "PDF/A2b Dokument");

        document.XmpMetadata = xmp;

        document.OutputIntents.Add(GeneratorVariables.ColorScheme);
    }

    private static void AddWatermark(Document document)
    {
        var textWatermark = new TextWatermark("Pouze pro informaci", GeneratorVariables.Arial.GetFont(), 68)
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