using CIS.Core.ErrorCodes;

namespace DomainServices.RealEstateValuationService.ExternalServices.PreorderService;

internal sealed class ErrorCodeMapper
    : ErrorCodeMapperBase
{
    public const int PreorderSvcUploadAttachmentNoFile = 1;

    public static IErrorCodesDictionary Init()
    {
        SetMessages(new Dictionary<int, string>()
        {
            { PreorderSvcUploadAttachmentNoFile, "Upload attachment returned empty response" }
        });

        return Messages;
    }
}
