using CIS.Core.ErrorCodes;

namespace DomainServices.RealEstateValuationService.ExternalServices.PreorderService;

internal sealed class ErrorCodeMapper
    : ErrorCodeMapperBase
{
    public const int PreorderSvcUploadAttachmentNoFile = 22100;
    public const int OrderOnlineBadRequest = 22204;
    public const int OrderStandardBadRequest = 22205;
    public const int OrderDtsBadRequest = 22206;

    public static IErrorCodesDictionary Init()
    {
        SetMessages(new Dictionary<int, string>()
        {
            { PreorderSvcUploadAttachmentNoFile, "Upload attachment returned empty response" }
        });

        return Messages;
    }
}
