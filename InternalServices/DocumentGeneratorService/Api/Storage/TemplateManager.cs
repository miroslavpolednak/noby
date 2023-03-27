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

    public async Task<PdfDocument> LoadTemplate(int documentTypeId, string templateVersion, string? templateVariant)
    {
        var versionId = await LoadTemplateVersionId(documentTypeId, templateVersion);
        var templateTypeName = await LoadTemplateTypeName(documentTypeId);

        await CheckTemplateVariant(versionId, templateVariant);

        return _fileStorage.LoadTemplateFile(templateTypeName, templateVersion, templateVariant);
    }

    public void DrawTemplate(Document template)
    {
        var memoryStream = new MemoryStream();

        template.Draw(memoryStream);

        _documentStreams.Add(memoryStream);
    }

    public async Task<FinalDocument> CreateFinalDocument(int documentTypeId, string templateVersion, string? templateVariant)
    {
        var document = await PrepareFinalDocument(documentTypeId, templateVersion, templateVariant);

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

    private async Task<int> LoadTemplateVersionId(int documentTypeId, string templateVersion)
    {
        var templateVersions = await _codebookService.DocumentTemplateVersions();

        var version = templateVersions.FirstOrDefault(t => t.DocumentTypeId == documentTypeId && t.DocumentVersion.Equals(templateVersion, StringComparison.InvariantCultureIgnoreCase)) ??
                      throw new CisValidationException(402, $"Unsupported template (id: {documentTypeId}) version {templateVersion}");

        return version.Id;
    }

    private async Task CheckTemplateVariant(int templateVersionId, string? templateVariant)
    {
        if (string.IsNullOrWhiteSpace(templateVariant))
            return;

        var variants = await _codebookService.DocumentTemplateVariants();

        var exists = variants.Any(v => v.DocumentTemplateVersionId == templateVersionId && v.DocumentVariant == templateVariant);

        if (!exists)
            throw new CisValidationException(402, $"Unsupported variant {templateVariant} (Version id: {templateVersionId})");
    }

    private async Task<MergeDocument> PrepareFinalDocument(int templateTypeId, string templateVersion, string? templateVariant)
    {
        var templateTypeName = await LoadTemplateTypeName(templateTypeId);

        return new MergeDocument
        {
            Title = $"{templateTypeName}_{templateVersion}{templateVariant}",
            Author = nameof(DocumentGeneratorService),
            Form = { Output = FormOutput.Flatten }
        };
    }
}