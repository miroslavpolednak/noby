using CIS.Core.ErrorCodes;

namespace DomainServices.RiskIntegrationService.Api;

internal sealed class ErrorCodeMapper
    : ErrorCodeMapperBase
{
    public const int ServiceUserIsNull = 17002;
    public const int ServiceUserNotFound = 17003;
    public const int UserIdentityIsNull = 17004;
    public const int UserInfoIsNull = 17005;
    public const int ProductTypeIdNotFound = 17006;
    public const int IncomeTypeIdNotFound = 17007;
    public const int ObligationTypeIdNotFound = 17008;
    public const int RiskApplicationTypeNotFound = 17009;
    public const int ApproverEqualDealer = 17010;
    public const int DealerSchemeIsNull = 17011;
    public const int ResourceIdentifierDealerSchemeIsNull = 17012;
    public const int C4MEnumNotFound = 17013;
    public const int BankCodeNotFound = 17014;
    public const int LoanPurposeIdNotFound = 17015;

    public static IErrorCodesDictionary Init()
    {
        SetMessages(new Dictionary<int, string>()
        {
            { ServiceUserIsNull, "ServiceUser2ItChannelBinding configuration is not set" },
            { ServiceUserNotFound, "ServiceUser '{PropertyValue}' not found in ServiceUser2ItChannelBinding configuration and no _default has been set" },
            { UserIdentityIsNull, "Can not obtain user information from XXV - identity is null" },
            { UserInfoIsNull, "Can not obtain user information from XXV for {PropertyValue}" },
            { ProductTypeIdNotFound, "ProductTypeId={PropertyValue} is missing in RiskApplicationTypes codebook" },
            { IncomeTypeIdNotFound, "IncomeType={PropertyValue} not found in IncomeMainTypes codebook" },
            { ObligationTypeIdNotFound, "ObligationTypeId={PropertyValue} does not exist" },
            { RiskApplicationTypeNotFound, "Can't find RiskApplicationType item" },
            { ApproverEqualDealer, "Approver can't be dealer" },
            { DealerSchemeIsNull, "IsKbGroupPerson() input parameter is null" },
            { ResourceIdentifierDealerSchemeIsNull, "GetResourceIdentifierInstanceForDealer() input parameter is null" },
            { C4MEnumNotFound, "Can't cast '{PropertyValue}' to C4M enum" },
            { BankCodeNotFound, "Transformation for BankCode={PropertyValue} does not exist" },
            { LoanPurposeIdNotFound, "Loan Purpose Id {PropertyValue} not found" }
        });

        return Messages;
    }
}
