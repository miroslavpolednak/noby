using CIS.Core.ErrorCodes;

namespace CIS.InternalServices.TaskSchedulingService.Api;

internal sealed class ErrorCodeMapper
    : ErrorCodeMapperBase
{
    public const int InstanceIsNotActive = 1;

    public static IErrorCodesDictionary Init()
    {
        SetMessages(new Dictionary<int, string>()
        {
            { InstanceIsNotActive, "This instance is not the active one. Please use '{PropertyValue}' instance for manual operations." }
        });

        return Messages;
    }
}
