using Microsoft.AspNetCore.Http;

namespace NOBY.Services.FileAntivirus;

public interface IFileAntivirusService
{
    Task<FileAntivirusResult> CheckFile(IFormFile file);

    Task<FileAntivirusResult> CheckFile(byte[] file);
}
