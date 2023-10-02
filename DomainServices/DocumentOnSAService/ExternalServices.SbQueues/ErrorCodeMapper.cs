using CIS.Core.ErrorCodes;

namespace ExternalServices.SbQueues;

internal sealed class ErrorCodeMapper : ErrorCodeMapperBase
{
    public static IErrorCodesDictionary Init()
    {
        SetMessages(new Dictionary<int, string>
        {
        });

        return Messages;
    }
}