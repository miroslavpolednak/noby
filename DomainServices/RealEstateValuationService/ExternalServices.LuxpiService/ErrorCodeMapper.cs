using CIS.Core.ErrorCodes;

namespace DomainServices.RealEstateValuationService.ExternalServices.LuxpiService;

internal sealed class ErrorCodeMapper
    : ErrorCodeMapperBase
{
    public const int LuxpiKbModelUnknownStatus = 22200;
    public const int LuxpiKbModelStatusFailed = 22202;
    public const int LuxpiKbModelIncorrectResult = 22203;

    public static IErrorCodesDictionary Init()
    {
        SetMessages(new Dictionary<int, string>()
        {
            { LuxpiKbModelUnknownStatus, "KB Model Status unknown: '{PropertyValue}'" },
            { LuxpiKbModelStatusFailed, "KB Model Status Knocked Out" },
            { LuxpiKbModelIncorrectResult, "KB Model did not respond correctly" }
        });

        return Messages;
    }
}
