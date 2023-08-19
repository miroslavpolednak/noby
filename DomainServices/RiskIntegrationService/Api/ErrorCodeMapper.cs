using CIS.Core.ErrorCodes;

namespace DomainServices.RiskIntegrationService.Api;

internal sealed class ErrorCodeMapper
    : ErrorCodeMapperBase
{
    public const int GeneralValidationError = 17000;
    public const int ServiceUserIsNull = 17001;
    public const int ServiceUserNotFound = 17002;
    public const int UserNotFound = 17003;
    public const int C4MEnumNotFound = 17004;
    public const int LoanPurposeIdNotFound = 17005;
    public const int ProductTypeIdNotFound = 17006;
    public const int IncomeTypeIdNotFound = 17007;
    public const int ObligationTypeIdNotFound = 17008;
    public const int RiskApplicationTypeNotFound = 17009;
    public const int ApproverEqualDealer = 17010;
    public const int BankCodeNotFound = 17011;

    public static IErrorCodesDictionary Init()
    {
        SetMessages(new Dictionary<int, string>()
        {
            { GeneralValidationError, "Value '{PropertyValue}' of property '{PropertyPath}' is not valid." },
            { ServiceUserIsNull, "ServiceUser2ItChannelBinding configuration is not set." },
            { ServiceUserNotFound, "ServiceUser '{PropertyValue}' not found in ServiceUser2ItChannelBinding configuration and no _default has been set." },
            { UserNotFound, "User '{PropertyValue}' not found." },
            { C4MEnumNotFound, "Can't cast '{PropertyValue}' to C4M enum." },
            { LoanPurposeIdNotFound, "Loan Purpose Id '{PropertyValue}' not found." },
            { ProductTypeIdNotFound, "Product Type Id '{PropertyValue}' not found." },
            { IncomeTypeIdNotFound, "Income Type Id '{PropertyValue}' not found." },
            { ObligationTypeIdNotFound, "Obligation Type Id '{PropertyValue}' not found." },
            { RiskApplicationTypeNotFound, "RiskApplicationType item not found." },
            { ApproverEqualDealer, "Approver can't be dealer." },
            { BankCodeNotFound, "Transformation for BankCode '{PropertyValue}' not found." },
        });

        return Messages;
    }
}
