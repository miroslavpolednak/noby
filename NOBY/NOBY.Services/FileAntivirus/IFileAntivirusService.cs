using Microsoft.AspNetCore.Http;

namespace NOBY.Services.FileAntivirus;

public interface IFileAntivirusService
{
    Task<CheckFileResults> CheckFile(IFormFile file);

    public enum CheckFileResults
    {
        Unknown,
        Passed,
        Timeouted,
        Failed
    }
}
