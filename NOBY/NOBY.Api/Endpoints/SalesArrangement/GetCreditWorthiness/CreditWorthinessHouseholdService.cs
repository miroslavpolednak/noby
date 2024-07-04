using CIS.Core;
using _Rip = DomainServices.RiskIntegrationService.Contracts.CreditWorthiness.V2;
using _HO = DomainServices.HouseholdService.Contracts;

#pragma warning disable CA1860 // Avoid using 'Enumerable.Any()' extension method

namespace NOBY.Api.Endpoints.SalesArrangement.GetCreditWorthiness;

[CIS.Core.Attributes.ScopedService, CIS.Core.Attributes.SelfService]
internal sealed class CreditWorthinessHouseholdService(
    DomainServices.HouseholdService.Clients.IHouseholdServiceClient _householdService,
    DomainServices.HouseholdService.Clients.ICustomerOnSAServiceClient _customerOnSaService)
{
    public async Task<List<_Rip.CreditWorthinessHousehold>> CreateHouseholds(int salesArrangementId, CancellationToken cancellationToken)
    {
        // seznam domacnosti na SA
        var households = await _householdService.GetHouseholdList(salesArrangementId, cancellationToken);
        if (households.Count == 0)
            throw new CisValidationException("There is no household bound for this SA");

        return (await households.Where(t => t.HouseholdTypeId == 1 || t.HouseholdTypeId == 2).SelectAsync(async household =>
        {
            var h = new _Rip.CreditWorthinessHousehold
            {
                ChildrenUpToTenYearsCount = household.Data.ChildrenUpToTenYearsCount.GetValueOrDefault(),
                ChildrenOverTenYearsCount = household.Data.ChildrenOverTenYearsCount.GetValueOrDefault(),
                ExpensesSummary = new()
                {
                    Rent = household.Expenses?.HousingExpenseAmount,
                    Saving = household.Expenses?.SavingExpenseAmount,
                    Insurance = household.Expenses?.InsuranceExpenseAmount,
                    Other = household.Expenses?.OtherExpenseAmount
                },
                Customers = []
            };

            // get customers
            int? maritalState1 = null;
            int? maritalState2 = null;
            if (household.CustomerOnSAId1.HasValue)
            {
                var c = await _customerOnSaService.GetCustomer(household.CustomerOnSAId1.Value, cancellationToken);
                h.Customers.Add(createClient(c));
                maritalState1 = c.MaritalStatusId;
            }
            if (household.CustomerOnSAId2.HasValue)
            {
                var c = await _customerOnSaService.GetCustomer(household.CustomerOnSAId2.Value, cancellationToken);
                h.Customers.Add(createClient(c));
                maritalState2 = c.MaritalStatusId;
            }

            bool isPartner = DomainServices.HouseholdService.Clients.Helpers.AreCustomersPartners(maritalState1, maritalState2);
            h.Customers.ForEach(t => t.HasPartner = isPartner);
            
            // Upravit validaci na FE API tak, aby hlídala, že aspoň jeden žadatel v každé z domácností na SA má vyplněný aspoň jeden příjem (=tedy nevalidovat, že každý žadatel musí mít vyplněný příjem)
            if (!h.Customers.Any(t => t.Incomes?.Any() ?? false))
                throw new CisValidationException("At least one customer in household must have some income");

            return h;
        })).ToList();
    }

    private static _Rip.CreditWorthinessCustomer createClient(_HO.CustomerOnSA customer)
    {
        var c = new _Rip.CreditWorthinessCustomer
        {
            PrimaryCustomerId = customer
                .CustomerIdentifiers
                .GetKbIdentityOrDefault()
                ?.IdentityId
                .ToString(System.Globalization.CultureInfo.InvariantCulture),
            MaritalStateId = customer.MaritalStatusId
        };

        if (customer.Incomes?.Any() ?? false)
            c.Incomes = customer.Incomes.Select(t => new _Rip.CreditWorthinessIncome
            {
                Amount = ((decimal?)t.Sum).GetValueOrDefault(0),
                IncomeTypeId = t.IncomeTypeId
            }).ToList();

        if (customer.Obligations?.Any() ?? false)
            c.Obligations = customer.Obligations.Select(t =>
            {
                if (t.ObligationTypeId == 3 || t.ObligationTypeId == 4)
                    return new _Rip.CreditWorthinessObligation
                    {
                        ObligationTypeId = t.ObligationTypeId.GetValueOrDefault(),
                        Amount = t.CreditCardLimit,
                        AmountConsolidated = t.Correction?.CreditCardLimitCorrection,
                        IsObligationCreditorExternal = t.Creditor?.IsExternal.GetValueOrDefault() ?? false
                    };
                else
                    return new _Rip.CreditWorthinessObligation
                    {
                        ObligationTypeId = t.ObligationTypeId.GetValueOrDefault(),
                        Installment = t.InstallmentAmount,
                        InstallmentConsolidated = t.Correction?.InstallmentAmountCorrection,
                        IsObligationCreditorExternal = t.Creditor?.IsExternal.GetValueOrDefault() ?? false
                    };
            }).ToList();

        return c;
    }
}
