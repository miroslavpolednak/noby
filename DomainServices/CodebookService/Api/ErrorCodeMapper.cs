using CIS.Core.ErrorCodes;

namespace DomainServices.CodebookService.Api;

internal sealed class ErrorCodeMapper
    : ErrorCodeMapperBase
{
    public const int DeveloperNotFound = 20000;
    public const int DeveloperProjectNotFound = 20001;
    public const int OperatorNotFound = 20002;
    public const int AcvRealEstateTypeNotFound = 20003;

    public static IErrorCodesDictionary Init()
    {
        SetMessages(new Dictionary<int, string>()
        {
            { DeveloperNotFound, "DeveloperId {PropertyValue} not found" },
            { DeveloperProjectNotFound, "DeveloperProjectId {PropertyValue} not found" },
            { OperatorNotFound, "Operator {PropertyValue} not found" },
            { AcvRealEstateTypeNotFound, "AcvRealEstateType not found" }
        });

        return Messages;
    }
}
