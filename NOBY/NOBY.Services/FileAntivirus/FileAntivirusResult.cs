using static NOBY.Services.FileAntivirus.FileAntivirusResult;

namespace NOBY.Services.FileAntivirus;

public sealed record class FileAntivirusResult(CheckFileResults Result, string? ErrorMessage = null)
{
    public enum CheckFileResults
    {
        Unknown,
        Passed,
        Timeouted,
        Failed
    }
}
