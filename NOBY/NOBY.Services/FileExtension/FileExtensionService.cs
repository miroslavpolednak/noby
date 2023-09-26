using NOBY.Infrastructure.ErrorHandling;

namespace NOBY.Services.FileExtension;

public interface IFileExtensionService
{
    void ValidateFileExtension(string fileExtension);
}

[SingletonService, AsImplementedInterfacesService]
public class FileExtensionService : IFileExtensionService
{
    private readonly HashSet<string> _allowedFileExtensions = new()
    {
        ".pdf",
        ".png",
        ".txt",
        ".xls",
        ".xlsx",
        ".doc",
        ".docx",
        ".rtf",
        ".jpg",
        ".jpeg",
        ".jfif",
        ".tif",
        ".tiff",
        ".gif"
    };

    public void ValidateFileExtension(string fileExtension)
    {
        if (!_allowedFileExtensions.Contains(fileExtension))
            throw new NobyValidationException($"Unsupported file extension {fileExtension}");
    }
}
