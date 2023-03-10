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

    public async Task<TemplateLoader> CreateLoader(int documentTypeId, string templateVersion)
    {
        await CheckTemplateVersion(documentTypeId, templateVersion);

        return new TemplateLoader(_fileStorage)
        {
            TemplateTypeName = await LoadTemplateTypeName(documentTypeId),
            TemplateVersion = templateVersion
        };
    }

    public void DrawTemplate(Document template)
    {
        var memoryStream = new MemoryStream();

        template.Draw(memoryStream);

        _documentStreams.Add(memoryStream);
    }

    public async Task<FinalDocument> CreateFinalDocument(int documentTypeId, string templateVersion)
    {
        var document = await PrepareFinalDocument(documentTypeId, templateVersion);

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
    }

    private async Task<string> LoadTemplateTypeName(int documentTypeId)
    {
        var templateTypes = await _codebookService.DocumentTypes();

        var type = templateTypes.FirstOrDefault(t => t.Id == documentTypeId) ??
                   throw new CisArgumentException(401, $"Unsupported template with Id {documentTypeId}", nameof(documentTypeId));

        return type.ShortName;
    }

    private async Task CheckTemplateVersion(int documentTypeId, string templateVersion)
    {
        var templateVersions = await _codebookService.DocumentTemplateVersions();

        var templateExists = templateVersions.Any(t => t.DocumentTypeId == documentTypeId &&
                                                       t.DocumentVersion.Equals(templateVersion, StringComparison.InvariantCultureIgnoreCase));

        if (!templateExists)
            throw new CisArgumentException(402, $"Unsupported template (id: {documentTypeId}) version {templateVersion}", nameof(templateVersion));
    }

    private async Task<MergeDocument> PrepareFinalDocument(int templateTypeId, string templateVersion)
    {
        await CheckTemplateVersion(templateTypeId, templateVersion);

        var templateTypeName = await LoadTemplateTypeName(templateTypeId);

        return new MergeDocument
        {
            Title = $"{templateTypeName}_{templateVersion}",
            Author = nameof(DocumentGeneratorService),
            Form = { Output = FormOutput.Flatten }
        };
    }
}