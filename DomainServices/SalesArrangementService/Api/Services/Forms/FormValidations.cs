using CIS.Infrastructure.gRPC.CisTypes;
using CIS.InternalServices.DataAggregator.EasForms;
using DomainServices.HouseholdService.Contracts;
using Household = CIS.InternalServices.DataAggregator.EasForms.FormData.ProductRequest.Household;

namespace DomainServices.SalesArrangementService.Api.Services.Forms;

internal static class FormValidations
{
    private const string StringJoinSeparator = ",";

    public static void CheckFormData(IProductFormData formData)
    {
        CheckArrangement(formData.SalesArrangement);

        CheckCustomersOnSA(formData.HouseholdData.CustomersOnSa);

        CheckHouseholds(formData.HouseholdData.Households, formData.HouseholdData.CustomersOnSa);

        CheckIncomes(formData.HouseholdData.Incomes);
    }

    private static void CheckArrangement(Contracts.SalesArrangement arrangement)
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
            throw new CisValidationException(18064, $"Sales arrangement mandatory fields not provided [{string.Join(StringJoinSeparator, invalidSaMandatoryFields)}].");
        }

        // check if Offer exists
        if (!arrangement.OfferId.HasValue)
        {
            throw new CisNotFoundException(18065, $"Sales Arrangement #{arrangement.SalesArrangementId} is not linked to Offer");
        }
    }

    private static void CheckIncomes(Dictionary<int, Income> incomesById)
    {
        // check mandatory fields of Incomes
        string[] FindInvalidFields(Income income)
        {
            // Kontrolují se pouze příjmy ze zaměstnání
            if (income.IncomeTypeId != 1)
            {
                return Array.Empty<string>();
            }

            var employmentMandatoryFields = new List<(string Field, bool Valid)>
            {
                //("EmploymentTypeId", (income.Employement?.Job?.EmploymentTypeId).HasValue  ),
                ("IsInProbationaryPeriod", (income.Employement?.Job?.IsInProbationaryPeriod).HasValue ),
                ("IsInTrialPeriod", (income.Employement?.Job?.IsInTrialPeriod).HasValue )
            };

            return employmentMandatoryFields.Where(i => !i.Valid).Select(i => i.Field).ToArray();
        }

        var invalidIncomes = incomesById.Select(i => new { Id = i.Key, InvalidFields = FindInvalidFields(i.Value) }).Where(i => i.InvalidFields.Length > 0).ToArray();
        if (invalidIncomes.Length > 0)
        {
            var details = invalidIncomes.Select(i => $"{i.Id}[{string.Join(StringJoinSeparator, i.InvalidFields)}]");
            throw new CisValidationException(18066, $"Income mandatory fields not provided [{string.Join(StringJoinSeparator, details)}].");
        }
    }

    private static void CheckCustomersOnSA(List<CustomerOnSA> customersOnSa)
    {
        // NOTE: v rámci Create/Update CustomerOnSA musí být vytvořena KB a MP identita !!!

        // check if each customer contains Mp identity and also Kb identity
        var customerIds = customersOnSa.Select(x => x.CustomerOnSAId).ToList();

        var customerIdentities = customersOnSa.SelectMany(c => c.CustomerIdentifiers.Select(i => new { c.CustomerOnSAId, Identity = i })).ToList();

        var customerIdsWithIdentityMp = customerIdentities.Where(i => i.Identity.IdentityScheme == Identity.Types.IdentitySchemes.Mp).Select(i => i.CustomerOnSAId);
        var customerIdsWithIdentityKb = customerIdentities.Where(i => i.Identity.IdentityScheme == Identity.Types.IdentitySchemes.Kb).Select(i => i.CustomerOnSAId);

        var customerIdsWithoutIdentityMp = customerIds.Where(id => !customerIdsWithIdentityMp.Contains(id));
        var customerIdsWithoutIdentityKb = customerIds.Where(id => !customerIdsWithIdentityKb.Contains(id));

        var customerIdsInvalid = customerIdsWithoutIdentityMp.Concat(customerIdsWithoutIdentityKb).ToList();

        if (customerIdsInvalid.Any())
        {
            throw new CisValidationException(18067, $"Sales arrangement customers [{string.Join(StringJoinSeparator, customerIdsInvalid)}] don't contain both [KB,MP] identities.");
        }
    }

    private static void CheckHouseholds(List<Household> households, List<CustomerOnSA> customersOnSa)
    {
        // check if each household type is represented at most once
        var duplicitHouseholdTypeIds = households.GroupBy(i => i.HouseholdType).Where(g => g.Count() > 1).Select(i => i.Key);
        if (duplicitHouseholdTypeIds.Any())
        {
            throw new CisValidationException(18068, $"Sales arrangement contains duplicit household types [{string.Join(StringJoinSeparator, duplicitHouseholdTypeIds)}].");
        }

        // check if MAIN household is available
        var mainHouseholdCount = households.Count(i => i.HouseholdType == CIS.Foms.Enums.HouseholdTypes.Main);
        if (mainHouseholdCount != 1)
        {
            throw new CisValidationException(18069, $"Sales arrangement must contain just one '{CIS.Foms.Enums.HouseholdTypes.Main}' household.");
        }

        // check if any household contains CustomerOnSAId2 without CustomerOnSAId1
        var invalidHouseholdIds = households.Where(i => !i.CustomerOnSaId1.HasValue && i.CustomerOnSaId2.HasValue).Select(i => i.HouseholdId);
        if (invalidHouseholdIds.Any())
        {
            throw new CisValidationException(18070, $"Sales arrangement contains households [{string.Join(StringJoinSeparator, invalidHouseholdIds)}] with CustomerOnSAId2 but without CustomerOnSAId1.");
        }

        // check if CustomerOnSAId1 is available on Main households
        var mainHousehold = households.Single(i => i.HouseholdType == CIS.Foms.Enums.HouseholdTypes.Main);
        if (!mainHousehold.CustomerOnSaId1.HasValue)
        {
            throw new CisValidationException(18071, $"Main household´s CustomerOnSAId1 not defined [{mainHousehold.HouseholdId}].");
        }

        // check if the same CustomerOnSA belongs to only one household
        var duplicitCustomerOnSAIds = households.Where(i => i.CustomerOnSaId1.HasValue).Select(i => i.CustomerOnSaId1!.Value)
           .Concat(households.Where(i => i.CustomerOnSaId2.HasValue).Select(i => i.CustomerOnSaId2!.Value))
           .GroupBy(i => i).Where(i => i.Count() > 1).Select(i => i.Key);
        if (duplicitCustomerOnSAIds.Any())
        {
            throw new CisValidationException(18072, $"Sales arrangement households contain duplicit customers [{string.Join(StringJoinSeparator, duplicitCustomerOnSAIds)}] on sales arrangement.");
        }

        // check if customers on SA correspond to customers on households
        var arrangementCustomerIds = customersOnSa.Select(i => i.CustomerOnSAId);
        var householdCustomerIds = households.Where(i => i.CustomerOnSaId1.HasValue).Select(i => i.CustomerOnSaId1!.Value)
            .Concat(households.Where(i => i.CustomerOnSaId2.HasValue).Select(i => i.CustomerOnSaId2!.Value));

        var missingCustomerIdsOnHouseholds = arrangementCustomerIds.Where(id => !householdCustomerIds.Contains(id));
        var missingCustomerIdsOnArrangement = householdCustomerIds.Where(id => !arrangementCustomerIds.Contains(id));

        var customerIdsInvalid = missingCustomerIdsOnHouseholds.Concat(missingCustomerIdsOnArrangement).ToList();

        if (customerIdsInvalid.Any())
        {
            throw new CisValidationException(18073, $"Customers [{string.Join(StringJoinSeparator, customerIdsInvalid)}] on sales arrangement don't correspond to customers on households.");
        }
    }
}