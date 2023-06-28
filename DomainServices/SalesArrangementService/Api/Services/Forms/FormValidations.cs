using CIS.Infrastructure.gRPC.CisTypes;
using CIS.InternalServices.DataAggregatorService.Contracts;

namespace DomainServices.SalesArrangementService.Api.Services.Forms;

internal static class FormValidations
{
    private const string StringJoinSeparator = ",";

    public static void CheckFormData(ProductData formData)
    {
        CheckCustomersOnSA(formData.CustomersOnSa);

        CheckHouseholds(formData.Households, formData.CustomersOnSa);

        CheckIncomes(formData.EmployementIncomes);
    }

    public static void CheckArrangement(Contracts.SalesArrangement arrangement)
    {
        // check mandatory fields of SalesArrangement
        var saMandatoryFields = new List<(string Field, bool Valid)>
        {
            ("IncomeCurrencyCode", !string.IsNullOrEmpty(arrangement.Mortgage?.IncomeCurrencyCode)  ),
            ("ResidencyCurrencyCode", !string.IsNullOrEmpty(arrangement.Mortgage?.ResidencyCurrencyCode) ),
            ("SignatureTypeId", (arrangement.Mortgage?.ContractSignatureTypeId).HasValue ),
        };

        var invalidSaMandatoryFields = saMandatoryFields.Where(i => !i.Valid).Select(i => i.Field).ToArray();

        if (invalidSaMandatoryFields.Length > 0)
        {
            throw ErrorCodeMapper.CreateValidationException(ErrorCodeMapper.FormValidation1, string.Join(StringJoinSeparator, invalidSaMandatoryFields));
        }

        // check if Offer exists
        if (!arrangement.OfferId.HasValue)
        {
            throw ErrorCodeMapper.CreateValidationException(ErrorCodeMapper.FormValidation2, arrangement.SalesArrangementId);
        }
    }

    private static void CheckCustomersOnSA(IList<ProductCustomerOnSa> customersOnSa)
    {
        // NOTE: v rámci Create/Update CustomerOnSA musí být vytvořena KB a MP identita !!!

        // check if each customer contains Mp identity and also Kb identity
        var customerIds = customersOnSa.Select(x => x.CustomerOnSaId).ToList();

        var customerIdentities = customersOnSa.SelectMany(c => c.Identities.Select(i => new { c.CustomerOnSaId, Identity = i })).ToList();

        var customerIdsWithIdentityMp = customerIdentities.Where(i => i.Identity.IdentityScheme == Identity.Types.IdentitySchemes.Mp).Select(i => i.CustomerOnSaId);
        var customerIdsWithIdentityKb = customerIdentities.Where(i => i.Identity.IdentityScheme == Identity.Types.IdentitySchemes.Kb).Select(i => i.CustomerOnSaId);

        var customerIdsWithoutIdentityMp = customerIds.Where(id => !customerIdsWithIdentityMp.Contains(id));
        var customerIdsWithoutIdentityKb = customerIds.Where(id => !customerIdsWithIdentityKb.Contains(id));

        var customerIdsInvalid = customerIdsWithoutIdentityMp.Concat(customerIdsWithoutIdentityKb).ToList();

        if (customerIdsInvalid.Any())
        {
            throw ErrorCodeMapper.CreateValidationException(ErrorCodeMapper.FormValidation3, string.Join(StringJoinSeparator, customerIdsInvalid));
        }
    }

    private static void CheckHouseholds(IList<ProductHousehold> households, IList<ProductCustomerOnSa> customersOnSa)
    {
        // check if each household type is represented at most once
        var duplicitHouseholdTypeIds = households.GroupBy(i => i.HouseholdTypeId).Where(g => g.Count() > 1).Select(i => i.Key);
        if (duplicitHouseholdTypeIds.Any())
        {
            throw ErrorCodeMapper.CreateValidationException(ErrorCodeMapper.FormValidation4, string.Join(StringJoinSeparator, duplicitHouseholdTypeIds));
        }

        // check if MAIN household is available
        var mainHouseholdCount = households.Count(i => i.HouseholdTypeId == (int)CIS.Foms.Enums.HouseholdTypes.Main);
        if (mainHouseholdCount != 1)
        {
            throw ErrorCodeMapper.CreateValidationException(ErrorCodeMapper.FormValidation5);
        }

        // check if any household contains CustomerOnSAId2 without CustomerOnSAId1
        var invalidHouseholdIds = households.Where(i => !i.CustomerOnSaId1.HasValue && i.CustomerOnSaId2.HasValue).Select(i => i.HouseholdId);
        if (invalidHouseholdIds.Any())
        {
            throw ErrorCodeMapper.CreateValidationException(ErrorCodeMapper.FormValidation6, string.Join(StringJoinSeparator, invalidHouseholdIds));
        }

        // check if CustomerOnSAId1 is available on Main households
        var mainHousehold = households.Single(i => i.HouseholdTypeId == (int)CIS.Foms.Enums.HouseholdTypes.Main);
        if (!mainHousehold.CustomerOnSaId1.HasValue)
        {
            throw ErrorCodeMapper.CreateValidationException(ErrorCodeMapper.FormValidation7, mainHousehold.HouseholdId);
        }

        // check if the same CustomerOnSA belongs to only one household
        var duplicitCustomerOnSAIds = households.Where(i => i.CustomerOnSaId1.HasValue).Select(i => i.CustomerOnSaId1!.Value)
           .Concat(households.Where(i => i.CustomerOnSaId2.HasValue).Select(i => i.CustomerOnSaId2!.Value))
           .GroupBy(i => i).Where(i => i.Count() > 1).Select(i => i.Key);
        if (duplicitCustomerOnSAIds.Any())
        {
            throw ErrorCodeMapper.CreateValidationException(ErrorCodeMapper.FormValidation8, string.Join(StringJoinSeparator, duplicitCustomerOnSAIds));
        }

        // check if customers on SA correspond to customers on households
        var arrangementCustomerIds = customersOnSa.Select(i => i.CustomerOnSaId);
        var householdCustomerIds = households.Where(i => i.CustomerOnSaId1.HasValue).Select(i => i.CustomerOnSaId1!.Value)
            .Concat(households.Where(i => i.CustomerOnSaId2.HasValue).Select(i => i.CustomerOnSaId2!.Value));

        var missingCustomerIdsOnHouseholds = arrangementCustomerIds.Where(id => !householdCustomerIds.Contains(id));
        var missingCustomerIdsOnArrangement = householdCustomerIds.Where(id => !arrangementCustomerIds.Contains(id));

        var customerIdsInvalid = missingCustomerIdsOnHouseholds.Concat(missingCustomerIdsOnArrangement).ToList();

        if (customerIdsInvalid.Any())
        {
            throw ErrorCodeMapper.CreateValidationException(ErrorCodeMapper.FormValidation10, string.Join(StringJoinSeparator, customerIdsInvalid));
        }
    }

    private static void CheckIncomes(IList<ProductEmployementIncome> incomes)
    {
        var invalidIncomes = incomes.Where(i => !i.IsInProbationaryPeriodHasValue || !i.IsInTrialPeriodHasValue).ToList();

        if (invalidIncomes.Count <= 0)
            return;

        var details = string.Join(StringJoinSeparator, invalidIncomes.Select(i => $"{i.IncomeId}"));
        throw ErrorCodeMapper.CreateValidationException(ErrorCodeMapper.FormValidation9, string.Join(StringJoinSeparator, details));
    }
}