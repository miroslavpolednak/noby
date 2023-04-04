using CIS.Core.ErrorCodes;

namespace DomainServices.DocumentArchiveService.ExternalServices.Tcp;
internal sealed class ErrorCodeMapper : ErrorCodeMapperBase
{
    public const int TcpDocumentNotExistOnUrl = 14018;
    public const int ToManyResultsFromExternalService = 9701;
    public const int TcpDocumentNotFound = 14002;

    public static IErrorCodesDictionary Init()
    {
        SetMessages(new Dictionary<int, string>()
        {
            { TcpDocumentNotExistOnUrl, "No document exist on specified url"},
            { ToManyResultsFromExternalService ,"To many results returned from external service, please specify filter more accurately"},
            { TcpDocumentNotFound ,"Unable to get/find document from eArchive (TCP)"}
        });

        return Messages;
    }
}
