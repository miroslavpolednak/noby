using ceTe.DynamicPDF.Merger;
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

    public Pdf.Document LoadTemplateFile(string templateName, string templateVersion)
    {
        var filePath = Path.Combine(_configuration.StoragePath, templateName, $"{templateName}_{templateVersion}.pdf");

        return LoadPdfDocument(filePath);
    }

    public void Dispose()
    {
        _fileStream?.Dispose();
    }

    private Pdf.Document LoadPdfDocument(string filePath)
    {
        _fileStream = File.OpenRead(filePath);

        return new MergeDocument(new PdfDocument(_fileStream));
    }
}