using ceTe.DynamicPDF.Forms;
using ceTe.DynamicPDF.Merger;
using CIS.Core.Exceptions;
using DomainServices.CodebookService.Clients;

namespace CIS.InternalServices.DocumentGeneratorService.Api.Storage;

[TransientService, SelfService]
public class TemplateManager : IDisposable
{
    private readonly TemplateFileStorage _fileStorage;
    private readonly ICodebookServiceClients _codebookService;

    private readonly List<Stream> _documentStreams = new();

    private string _templateTypeName = null!;
    private string _templateVersion = null!;

    public TemplateManager(TemplateFileStorage fileStorage, ICodebookServiceClients codebookService)
    {
        _fileStorage = fileStorage;
        _codebookService = codebookService;
    }

    public async Task<Pdf.Document> LoadTemplate(int templateTypeId, string templateVersion)
    {
        await CheckTemplateVersion(templateTypeId, templateVersion);

        _templateTypeName = await LoadTemplateTypeName(templateTypeId);
        _templateVersion = templateVersion;

        return _fileStorage.LoadTemplateFile(_templateTypeName, _templateVersion);
    }

    public void DrawTemplate(Pdf.Document template)
    {
        var memoryStream = new MemoryStream();

        template.Draw(memoryStream);

        _documentStreams.Add(memoryStream);
    }

    public Pdf.Document CreateFinalDocument()
    {
        var document = new MergeDocument
        {
            Title = $"{_templateTypeName}_{_templateVersion}",
            Author = nameof(DocumentGeneratorService),
            Form = { Output = FormOutput.Flatten }
        };

        _documentStreams.ForEach(stream =>
        {
            document.Append(new PdfDocument(stream));
        });

        return document;
    }

    public void Dispose()
    {
        _documentStreams.ForEach(stream => stream.Dispose());
    }

    private async Task<string> LoadTemplateTypeName(int templateTypeId)
    {
        var templateTypes = await _codebookService.DocumentTemplateTypes();

        var type = templateTypes.FirstOrDefault(t => t.Id == templateTypeId) ??
                   throw new CisArgumentException(401, $"Unsupported template with Id {templateTypeId}", nameof(templateTypeId));

        return type.ShortName;
    }

    private async Task CheckTemplateVersion(int templateTypeId, string templateVersion)
    {
        var templateVersions = await _codebookService.DocumentTemplateVersions();

        var templateExists = templateVersions.Any(t => t.DocumentTemplateTypeId == templateTypeId &&
                                                       t.DocumentVersion.Equals(templateVersion, StringComparison.InvariantCultureIgnoreCase));

        if (!templateExists)
            throw new CisArgumentException(402, $"Unsupported template (id: {templateTypeId}) version {templateVersion}", nameof(templateVersion));
    }
}