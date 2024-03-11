using CIS.Core.ErrorCodes;

namespace ExternalServices.SbWebApi;

internal sealed class ErrorCodeMapper
    : ErrorCodeMapperBase
{
    public const int ContractNumberSbNotFound = 13031;
    public const int TaskIdNotFound = 13026;
    public const int RefinancingError = 13037;

    public static IErrorCodesDictionary Init()
    {
        SetMessages(new Dictionary<int, string>()
        {
            { ContractNumberSbNotFound, "Contract number not found in SB" },
            { TaskIdNotFound, "TaskId {PropertyValue} not found" }
        });

        return Messages;
    }
}
