using CIS.Core.ErrorCodes;

namespace DomainServices.RiskIntegrationService.Api;

internal sealed class ErrorCodeMapper
    : ErrorCodeMapperBase
{
    public const int xxx = 1;

    public static IErrorCodesDictionary Init()
    {
        SetMessages(new Dictionary<int, string>()
        {
            { xxx, "xxx" }
        });

        return Messages;
    }
}
