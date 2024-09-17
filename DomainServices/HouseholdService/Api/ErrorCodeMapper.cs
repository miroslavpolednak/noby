using Amazon.Runtime.Internal.Transform;
using CIS.Core.ErrorCodes;

namespace DomainServices.HouseholdService.Api;

internal sealed class ErrorCodeMapper
    : ErrorCodeMapperBase
{
    public const int SalesArrangementIdIsEmpty =        16010;
    public const int IdentityAlreadyExistOnCustomer =   16011;
    public const int CustomerNotOnSA =                  16019;
    public const int CustomerOnSANotFound =             16020;
    public const int CustomerRoleNotFound =             16021;
    public const int HouseholdNotFound =                16022;
    public const int HouseholdTypeIdNotFound =          16023;
    public const int CustomerOnSAIdIsEmpty =            16024;
    public const int HouseholdTypeIdIsEmpty =           16027;
    public const int IncomeTypeIdIsEmpty =              16028;
    public const int IncomeNotFound =                   16029;
    public const int CurrencyNotValid =                 16030;
    public const int MoreDebtorHouseholds =             16031;
    public const int CantDeleteDebtorHousehold =        16032;
    public const int InvalidDateOfBirth =               16038;
    public const int ObligationNotFound =               16042;
    public const int CustomerRoleIdIsEmpty =            16045;
    public const int EmployementCinBirthNo =            16046;
    public const int MaxIncomesReached =                16047;
    public const int ObligationTypeIsEmpty =            16048;
    public const int CreditCardLimitNotAllowed =        16049;
    public const int LoanPrincipalAmountNotAllowed =    16050;
    public const int InstallmentAmountNotAllowed =      16051;
    public const int CreditorIdAndNameInSameTime =      16052;
    public const int IncomeIdIsEmpty =                  16055;
    public const int Customer2WithoutCustomer1 =        16056;
    public const int HouseholdIdIsEmpty =               16080;
    public const int EasKbDifference =                  16082;
    public const int TwoSameIncomes =                   16083;

    public static IErrorCodesDictionary Init()
    {
        SetMessages(new Dictionary<int, string>()
        {
            { SalesArrangementIdIsEmpty, "SalesArrangementId must be > 0" },
            { IdentityAlreadyExistOnCustomer, "Some of request identities already exists on CustomerOnSA with different ID" },
            { CustomerNotOnSA, "CustomerOnSA {PropertyValue} does not exist in selected SA." },
            { CustomerOnSANotFound , "Customer ID {PropertyValue} not found" },
            { CustomerRoleNotFound, "CustomerRoleId {PropertyValue} does not exist." },
            { HouseholdNotFound, "Household ID {PropertyValue} does not exist." },
            { HouseholdTypeIdNotFound, "HouseholdTypeId {PropertyValue} does not exist." },
            { CustomerOnSAIdIsEmpty, "CustomerOnSAId must be > 0" },
            { HouseholdTypeIdIsEmpty, "HouseholdTypeId must be > 0" },
            { IncomeTypeIdIsEmpty , "IncomeTypeId must be > 0" },
            { IncomeNotFound, "Income ID {PropertyValue} does not exist." },
            { CurrencyNotValid , "CurrencyId is not valid" },
            { MoreDebtorHouseholds, "Only one Debtor household allowed" },
            { CantDeleteDebtorHousehold, "Can't delete Debtor household" },
            { InvalidDateOfBirth, "Date of birth is out of range" },
            { ObligationNotFound, "Obligation ID {PropertyValue} does not exist." },
            { CustomerRoleIdIsEmpty, "CustomerRoleId must be > 0" },
            { EmployementCinBirthNo , "Only one of values can be set [Employement.Employer.Cin, Employement.Employer.BirthNumber]" },
            { MaxIncomesReached, "Max incomes of the type {PropertyValue} has been reached" },
            { ObligationTypeIsEmpty, "ObligationTypeId is not valid" },
            { CreditCardLimitNotAllowed, "CreditCardLimit not allowed for current ObligationTypeId" },
            { LoanPrincipalAmountNotAllowed, "LoanPrincipalAmount not allowed for current ObligationTypeId" },
            { InstallmentAmountNotAllowed, "InstallmentAmount not allowed for current ObligationTypeId" },
            { CreditorIdAndNameInSameTime, "Creditor.CreditorId and Creditor.Name can't be set in the same time" },
            { IncomeIdIsEmpty, "IncomeId must be > 0" },
            { Customer2WithoutCustomer1, "CustomerOnSAId1 is not set although CustomerOnSAId2 is." },
            { HouseholdIdIsEmpty, "HouseholdId must be > 0" },
            { TwoSameIncomes, "Multiple employment incomes with the same birth number / cin." }
        });

        return Messages;
    }
}
