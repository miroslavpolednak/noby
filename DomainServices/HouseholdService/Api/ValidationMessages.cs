using System.Collections.Immutable;

namespace DomainServices.HouseholdService.Api;

internal static class ValidationMessages
{
    public const int CustomerOnSAIdIsEmpty = 16024;
    public const int IncomeTypeIdIsEmpty = 16028;
    public const int EmployementCinBirthNo = 16046;
    public const int CustomerOnSANotFound = 16020;
    public const int CurrencyNotValid = 16030;
    public const int SalesArrangementIdIsEmpty = 16010;
    public const int HouseholdTypeIdIsEmpty = 16027;
    public const int Customer2WithoutCustomer1 = 16056;
    public const int MoreDebtorHouseholds = 16031;
    public const int HouseholdTypeIdNotFound = 16023;
    public const int HouseholdNotFound = 16022;
    public const int CantDeleteDebtorHousehold = 16032;
    public const int CustomerNotOnSA = 16019;
    public const int HouseholdIdIsEmpty = 16080;

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
        { HouseholdIdIsEmpty, "HouseholdId must be > 0" }
    }).ToImmutableSortedDictionary();

    public static string GetFormattedMessage(int key, object propertyValue)
        => Messages[key].Replace("{PropertyValue}", propertyValue.ToString());
}
