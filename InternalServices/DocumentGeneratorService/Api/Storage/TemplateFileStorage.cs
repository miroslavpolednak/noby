using Microsoft.Extensions.Options;

namespace CIS.InternalServices.DocumentGeneratorService.Api.Storage;

[TransientService, SelfService]
public class TemplateFileStorage : IDisposable
{
    private Stream? _fileStream;

    public TemplateFileStorage(IOptions<GeneratorConfiguration> configurationOptions)
    {
        StoragePath = configurationOptions.Value.StoragePath;
    }

    public string StoragePath { get; }

    public PdfDocument LoadTemplateFile(string templateName, string templateVersion, string? templateVariant)
    {
        var filePath = Path.Combine(StoragePath, templateName, $"{templateName}_{templateVersion}{templateVariant}.pdf");

        return LoadPdfDocument(filePath);
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        _fileStream?.Dispose();
    }

    private PdfDocument LoadPdfDocument(string filePath)
    {
        _fileStream = File.OpenRead(filePath);

        return new PdfDocument(_fileStream);
    }
}