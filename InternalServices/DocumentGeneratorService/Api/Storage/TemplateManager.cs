using CIS.Core.Exceptions;
using DomainServices.CodebookService.Clients;

namespace CIS.InternalServices.DocumentGeneratorService.Api.Storage;

[TransientService, SelfService]
public class TemplateManager : IDisposable
{
    private readonly TemplateFileStorage _fileStorage;
    private readonly ICodebookServiceClients _codebookService;

    private readonly List<Stream> _documentStreams = new();

    public TemplateManager(TemplateFileStorage fileStorage, ICodebookServiceClients codebookService)
    {
        _fileStorage = fileStorage;
        _codebookService = codebookService;
    }

    public string StoragePath => _fileStorage.StoragePath;

    public async Task<PdfDocument> LoadTemplate(int documentTypeId, int templateVersionId, int? templateVariantId)
    {
        var templateTypeName = await LoadTemplateTypeName(documentTypeId);
        var versionName = await LoadTemplateVersionName(documentTypeId, templateVersionId);
        var variantName = await LoadTemplateVariantName(templateVersionId, templateVariantId);

        return _fileStorage.LoadTemplateFile(templateTypeName, versionName, variantName);
    }

    public void DrawTemplate(Document template)
    {
        var memoryStream = new MemoryStream();

        template.Draw(memoryStream);

        _documentStreams.Add(memoryStream);
    }

    public async Task<FinalDocument> CreateFinalDocument(int documentTypeId, int templateVersionId, int? templateVariantId)
    {
        var document = await PrepareFinalDocument(documentTypeId, templateVersionId, templateVariantId);

        var finalDocument = new FinalDocument(document);

        _documentStreams.ForEach(stream =>
        {
            var pdfDocument = new PdfDocument(stream);

            document.Append(pdfDocument);
            finalDocument.PdfDocumentParts.Add(pdfDocument);
        });

        return finalDocument;
    }

    public void Dispose()
    {
        _documentStreams.ForEach(stream => stream.Dispose());
        _fileStorage.Dispose();
    }

    private async Task<string> LoadTemplateTypeName(int documentTypeId)
    {
        var templateTypes = await _codebookService.DocumentTypes();

        var type = templateTypes.FirstOrDefault(t => t.Id == documentTypeId) ??
                   throw new CisValidationException(401, $"Unsupported template with Id {documentTypeId}");

        return type.ShortName;
    }

    private async Task<string> LoadTemplateVersionName(int documentTypeId, int templateVersionId)
    {
        var templateVersions = await _codebookService.DocumentTemplateVersions();

        var version = templateVersions.FirstOrDefault(t => t.DocumentTypeId == documentTypeId && t.Id == templateVersionId)
                      ?? throw new CisValidationException(402, $"Unsupported template (id: {documentTypeId}) version id {templateVersionId}");

        return version.DocumentVersion;
    }

    private async Task<string> LoadTemplateVariantName(int templateVersionId, int? templateVariantId)
    {
        if (!templateVariantId.HasValue)
            return string.Empty;

        var templateVariants = await _codebookService.DocumentTemplateVariants();

        var variant = templateVariants.FirstOrDefault(v => v.DocumentTemplateVersionId == templateVersionId && v.Id == templateVariantId)
                      ?? throw new CisValidationException(402, $"Unsupported variant {templateVariantId} (Version id: {templateVersionId})");

        return variant.DocumentVariant;
    }

    private async Task<MergeDocument> PrepareFinalDocument(int templateTypeId, int templateVersionId, int? templateVariantId)
    {
        var typeName = await LoadTemplateTypeName(templateTypeId);
        var versionName = await LoadTemplateVersionName(templateTypeId, templateVersionId);
        var variantName = await LoadTemplateVariantName(templateVersionId, templateVariantId);

        return new MergeDocument
        {
            Title = $"{typeName}_{versionName}{variantName}",
            Author = nameof(DocumentGeneratorService),
            Form = { Output = FormOutput.Flatten }
        };
    }
}