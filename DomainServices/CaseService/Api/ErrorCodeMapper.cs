using Amazon.Runtime.Internal.Transform;
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
    public const int CaseStateNotFound = 13011;
    public const int CustomerNameIsEmpty = 13012;
    public const int ContractNumberIsEmpty = 13013;
    public const int ProductTypeIdNotFound = 13014;
    public const int CaseAlreadyExist = 13015;
    public const int CaseIdIsEmpty = 13016;
    public const int InvalidCaseState = 13017;
    public const int TargetAmountIsEmpty = 13018;
    public const int AuthenticatedUserNotFound = 13019;
    public const int CantDeleteCase = 13021;
    public const int TaskIdNotFound = 13026;
    public const int TaskTypeIdIsEmpty = 13027;
    public const int ProcessIdIsEmpty = 13028;
    public const int TaskTypeIdNotAllowed = 13029;
    public const int TaskIdSBIsEmpty = 13030;
    public const int ContractNumberNotFound = 13032;
    public const int TaskPriceExceptionIsEmpty = 13034;
    public const int CaseCancelled = 13035;
    public const int UnableToCancelCase = 13036;
    public const int InterestRateValidFromEmpty = 13037;
    public const int LoanInterestRateEmpty = 13038;
    public const int LoanInterestRateProvidedEmpty = 13039;
    public const int LoanPaymentAmountEmpty = 13040;
    public const int LoanPaymentAmountFinalEmpty = 13041;
    public const int FeeSumEmpty = 13042;
    public const int FeeFinalSumEmpty = 13043;
    public const int RetentionNull = 13044;
    public static IErrorCodesDictionary Init()
    {
        SetMessages(new Dictionary<int, string>()
        {
            { CaseNotFound, "Case ID {PropertyValue} not found" },
            { TaskProcessIdNotUnique, "TaskProcessId must be unique" },
            { ProductTypeIdIsEmpty, "ProductTypeId must be > 0" },
            { CaseOwnerIsEmpty, "CaseOwnerUserId must be > 0" },
            { CaseStateAlreadySet, "Case state already set to the same value" },
            { CaseStateNotAllowed, "Case state change not allowed" },
            { CaseStateNotFound, "State {PropertyValue} not found" },
            { CustomerNameIsEmpty, "Customer Name must not be empty" },
            { ContractNumberIsEmpty, "Contract number is empty" },
            { ProductTypeIdNotFound, "ProductTypeId {PropertyValue} not found" },
            { CaseAlreadyExist, "Case ID {PropertyValue} already exists" },
            { CaseIdIsEmpty, "CaseId must be > 0" },
            { InvalidCaseState, "Case State must be > 0" },
            { TargetAmountIsEmpty, "Target amount must be > 0" },
            { AuthenticatedUserNotFound, "Authenticated user has not been passed in auth headers" },
            { CantDeleteCase, "Unable to delete Case – one or more SalesArrangements exists for this case" },
            { TaskIdNotFound, "TaskId {PropertyValue} not found" },
            { TaskTypeIdIsEmpty, "TaskTypeId must be > 0" },
            { ProcessIdIsEmpty, "ProcessId must be > 0" },
            { TaskTypeIdNotAllowed, "TaskTypeId is not allowed" },
            { TaskIdSBIsEmpty, "TaskIdSB must be > 0" },
            { ContractNumberNotFound, "Contract number not found" },
            { TaskPriceExceptionIsEmpty, "Task PriceException must not be null for task type ID 2" },
            { CaseCancelled, "Case state is one of cancelled" },
            { UnableToCancelCase, "Unable to cancel Case {PropertyValue}" },
            { InterestRateValidFromEmpty, "InterestRateValidFrom cannot be empty" },
            { LoanInterestRateEmpty, "LoanInterestRateEmpty cannot be empty"},
            { LoanInterestRateProvidedEmpty, "LoanInterestRateProvided cannot be empty" },
            { LoanPaymentAmountEmpty, "LoanPaymentAmount cannot be empty" },
            { LoanPaymentAmountFinalEmpty, "LoanPaymentAmountFinal cannot be empty" },
            { FeeSumEmpty ,"FeeSumEmpty cannot be empty" },
            { FeeFinalSumEmpty, "FeeFinalSumEmpty cannot be empty" },
            { RetentionNull, "Retention cannot be null"}
        });

        return Messages;
    }
}
