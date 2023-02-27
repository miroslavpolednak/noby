using System.Collections.Immutable;

namespace DomainServices.HouseholdService.Api;

internal static class ValidationMessages
{
    public const int SalesArrangementIdIsEmpty =        16010;
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
    public const int Customer2WithoutCustomer1 =        16056;
    public const int HouseholdIdIsEmpty =               16080;
    public const int CantDeleteDebtor =                 16053;
    
    public static ImmutableSortedDictionary<int, string> Messages = (new Dictionary<int, string>()
    {
        { CustomerOnSAIdIsEmpty, "CustomerOnSAId must be > 0" },
        { IncomeTypeIdIsEmpty , "IncomeTypeId must be > 0" },
        { EmployementCinBirthNo , "Only one of values can be set [Employement.Employer.Cin, Employement.Employer.BirthNumber]" },
        { CustomerOnSANotFound , "Customer ID {PropertyValue} not found" },
        { CurrencyNotValid , "CurrencyId is not valid" },
        { SalesArrangementIdIsEmpty, "SalesArrangementId must be > 0" },
        { HouseholdTypeIdIsEmpty, "HouseholdTypeId must be > 0" },
        { Customer2WithoutCustomer1, "CustomerOnSAId1 is not set although CustomerOnSAId2 is." },
        { MoreDebtorHouseholds, "Only one Debtor household allowed" },
        { HouseholdTypeIdNotFound, "HouseholdTypeId {PropertyValue} does not exist." },
        { HouseholdNotFound, "Household ID {PropertyValue} does not exist." },
        { CantDeleteDebtorHousehold, "Can't delete Debtor household" },
        { CustomerNotOnSA, "CustomerOnSA #1 ID {PropertyValue} does not exist in selected SA." },
        { HouseholdIdIsEmpty, "HouseholdId must be > 0" },
        { CustomerRoleIdIsEmpty, "CustomerRoleId must be > 0" },
        { CustomerRoleNotFound, "CustomerRoleId {PropertyValue} does not exist." },
        { InvalidDateOfBirth, "Date of birth is out of range" },
        { MaxIncomesReached, "Max incomes of the type {PropertyValue} has been reached" },
        { ObligationTypeIsEmpty, "ObligationTypeId is not valid" },
        { CreditCardLimitNotAllowed, "CreditCardLimit not allowed for current ObligationTypeId" },
        { LoanPrincipalAmountNotAllowed, "LoanPrincipalAmount not allowed for current ObligationTypeId" },
        { InstallmentAmountNotAllowed, "InstallmentAmount not allowed for current ObligationTypeId" },
        { CreditorIdAndNameInSameTime, "Creditor.CreditorId and Creditor.Name can't be set in the same time" },
        { CantDeleteDebtor, "CustomerOnSA is in role=Debtor -> can't be deleted" },
        { IncomeNotFound, "Income ID {PropertyValue} does not exist." },
        { ObligationNotFound, "Obligation ID {PropertyValue} does not exist." }
    }).ToImmutableSortedDictionary();

    public static string GetFormattedMessage(int key, object propertyValue)
        => Messages[key].Replace("{PropertyValue}", propertyValue.ToString());
}
