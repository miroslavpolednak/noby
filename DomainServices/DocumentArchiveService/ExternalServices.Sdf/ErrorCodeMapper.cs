using CIS.Core.ErrorCodes;

namespace DomainServices.DocumentArchiveService.ExternalServices.Sdf;
internal sealed class ErrorCodeMapper : ErrorCodeMapperBase
{
    public const int CspDocumentNotFound = 14003;

    public static IErrorCodesDictionary Init()
    {
        SetMessages(new Dictionary<int, string>()
        {
            { CspDocumentNotFound, "Unable to get/find document(s) from eArchive (CSP/SDF)"},
        });

        return Messages;
    }
}
