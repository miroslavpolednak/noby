namespace CIS.InternalServices.DocumentGeneratorService.Api.Storage;

public class TemplateLoader
{
    private readonly TemplateFileStorage _storage;
    private readonly Dictionary<string, PdfDocument> _loadedDocuments = new();

    public TemplateLoader(TemplateFileStorage storage)
    {
        _storage = storage;
    }

    public required string TemplateTypeName { get; init; }

    public required string TemplateVersion { get; init; }

    public PdfDocument Load(string? modifier = null)
    {
        if (_loadedDocuments.ContainsKey(modifier ?? string.Empty))
            return _loadedDocuments[modifier ?? string.Empty];

        var document = _storage.LoadTemplateFile(TemplateTypeName, TemplateVersion, modifier);
        _loadedDocuments.Add(modifier ?? string.Empty, document);

        return document;
    }
}