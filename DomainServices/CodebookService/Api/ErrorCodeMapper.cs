using CIS.Core.ErrorCodes;

namespace DomainServices.CodebookService.Api;

internal sealed class ErrorCodeMapper
    : ErrorCodeMapperBase
{
    public const int DeveloperNotFound = 20000;
    public const int DeveloperProjectNotFound = 20001;

    public static IErrorCodesDictionary Init()
    {
        SetMessages(new Dictionary<int, string>()
        {
            { DeveloperNotFound, "Developer not found" },
            { DeveloperProjectNotFound, "Developer project not found" }
        });

        return Messages;
    }
}
