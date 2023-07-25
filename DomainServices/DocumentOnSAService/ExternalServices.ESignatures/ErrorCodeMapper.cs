using CIS.Core.ErrorCodes;

namespace ExternalServices.ESignatures;

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