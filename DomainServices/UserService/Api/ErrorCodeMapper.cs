using CIS.Core.ErrorCodes;

namespace DomainServices.UserService.Api;

internal sealed class ErrorCodeMapper
    : ErrorCodeMapperBase
{
    public const int UserNotFound = 21000;

    public static IErrorCodesDictionary Init()
    {
        SetMessages(new Dictionary<int, string>()
        {
            { UserNotFound, "User {PropertyValue} not found" },
        });

        return Messages;
    }
}
