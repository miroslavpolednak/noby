using Microsoft.Extensions.Options;

namespace CIS.InternalServices.DocumentGeneratorService.Api.Storage;

[TransientService, SelfService]
public class TemplateFileStorage : IDisposable
{
    private readonly GeneratorConfiguration _configuration;

    private Stream? _fileStream;

    public TemplateFileStorage(IOptions<GeneratorConfiguration> configurationOptions)
    {
        _configuration = configurationOptions.Value;
    }

    public PdfDocument LoadTemplateFile(string templateName, string templateVersion, string? modifier)
    {
        modifier = string.IsNullOrWhiteSpace(modifier) ? null : $"_{modifier}";

        var filePath = Path.Combine(_configuration.StoragePath, templateName, $"{templateName}_{templateVersion}{modifier}.pdf");

        return LoadPdfDocument(filePath);
    }

    public void Dispose()
    {
        _fileStream?.Dispose();
    }

    private PdfDocument LoadPdfDocument(string filePath)
    {
        _fileStream = File.OpenRead(filePath);

        return new PdfDocument(_fileStream);
    }
}