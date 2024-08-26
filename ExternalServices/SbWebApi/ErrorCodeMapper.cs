using CIS.Core.ErrorCodes;

namespace ExternalServices.SbWebApi;

internal sealed class ErrorCodeMapper
    : ErrorCodeMapperBase
{
    public const int ContractNumberSbNotFound = 13031;
    public const int TaskIdNotFound = 13026;
    public const int RefinancingError = 13037;
    public const int SbMissingContacts = 332;
    public static IErrorCodesDictionary Init()
    {
        SetMessages(new Dictionary<int, string>()
        {
            { ContractNumberSbNotFound, "Contract number not found in SB" },
            { TaskIdNotFound, "TaskId was not found" },
            { RefinancingError, "Not possible to have more active Retention/Refixation processes" },
            { SbMissingContacts, "Missing contacts info in SB" }
        });

        return Messages;
    }
}
