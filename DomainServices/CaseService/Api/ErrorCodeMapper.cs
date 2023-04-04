using CIS.Core.ErrorCodes;

namespace DomainServices.CaseService.Api;

internal sealed class ErrorCodeMapper
    : ErrorCodeMapperBase
{
    public const int CaseNotFound = 13000;
    public const int TaskProcessIdNotUnique = 13001;
    public const int ProductTypeIdIsEmpty = 13002;
    public const int CaseOwnerIsEmpty = 13003;
    public const int CaseStateAlreadySet = 13005;
    public const int CaseStateNotAllowed = 13006;
    public const int WfTaskValidationFailed1 = 13008;
    public const int WfTaskValidationFailed2 = 13009;
    public const int CaseStateNotFound = 13011;
    public const int CustomerNameIsEmpty = 13012;
    public const int ProductTypeIdNotFound = 13014;
    public const int CaseAlreadyExist = 13015;
    public const int CaseIdIsEmpty = 13016;
    public const int InvalidCaseState = 13017;
    public const int TargetAmountIsEmpty = 13018;
    public const int AuthenticatedUserNotFound = 13019;
    public const int CantDeleteCase = 13021;
    public const int RequestNotFoundInCache = 13024;
    public const int KafkaMessageIncorrectFormat = 13025;

    public static IErrorCodesDictionary Init()
    {
        SetMessages(new Dictionary<int, string>()
        {
            { ProductTypeIdIsEmpty, "ProductTypeId must be > 0" },
            { TargetAmountIsEmpty, "Target amount must be > 0" },
            { CaseOwnerIsEmpty, "CaseOwnerUserId must be > 0" },
            { CustomerNameIsEmpty, "Customer Name must not be empty" },
            { CaseAlreadyExist, "Case ID {PropertyValue} already exists" },
            { CaseIdIsEmpty, "CaseId must be > 0" },
            { CaseNotFound, "Case ID {PropertyValue} not found" },
            { CantDeleteCase, "Unable to delete Case – one or more SalesArrangements exists for this case" },
            { WfTaskValidationFailed1, "Found tasks [{PropertyValue}] with invalid TypeId." },
            { WfTaskValidationFailed2, "Found tasks [{PropertyValue}] with invalid StateId." },
            { TaskProcessIdNotUnique, "TaskProcessId must be unique" },
            { ProductTypeIdNotFound, "ProductTypeId {PropertyValue} not found" },
            { InvalidCaseState, "Case State must be > 0" },
            { CaseStateNotFound, "State {PropertyValue} not found" },
            { CaseStateAlreadySet, "Case state already set to the same value" },
            { CaseStateNotAllowed, "Case state change not allowed" },
            { AuthenticatedUserNotFound, "Authenticated user has not been passed in auth headers" },
            { RequestNotFoundInCache, "RequestId {PropertyValue} not found in cache" },
            { KafkaMessageIncorrectFormat, "Message CaseId {PropertyValue} is not in valid format" }

        });

        return Messages;
    }
}
