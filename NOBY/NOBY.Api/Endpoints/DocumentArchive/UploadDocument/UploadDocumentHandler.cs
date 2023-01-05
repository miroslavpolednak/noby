using NOBY.Infrastructure.Configuration;

namespace NOBY.Api.Endpoints.DocumentArchive.UploadDocument;

public class UploadDocumentHandler : IRequestHandler<UploadDocumentRequest, Guid>
{
    private readonly AppConfiguration _configuration;

    public UploadDocumentHandler(AppConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<Guid> Handle(UploadDocumentRequest request, CancellationToken cancellationToken)
    {
        if (request.File is null || request.File.Length <= 0)
        {
            throw new CisValidationException("No file has been uploaded");
        }

        if (!Directory.Exists(_configuration.FileTempFolderLocation))
            Directory.CreateDirectory(_configuration.FileTempFolderLocation);

        var tempFilename = Guid.NewGuid();
        var fullPath = Path.Combine(_configuration.FileTempFolderLocation, tempFilename.ToString());

        using var stream = new FileStream(fullPath, FileMode.Create);
        await request.File.CopyToAsync(stream,cancellationToken);
        await stream.FlushAsync(cancellationToken);

        return tempFilename;
    }
}
