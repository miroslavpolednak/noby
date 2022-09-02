using CIS.Core;
using _Rip = DomainServices.RiskIntegrationService.Contracts.CreditWorthiness.V2;
using _SA = DomainServices.SalesArrangementService.Contracts;

namespace FOMS.Api.Endpoints.SalesArrangement.GetCreditWorthiness;

[CIS.Infrastructure.Attributes.ScopedService, CIS.Infrastructure.Attributes.SelfService]
internal sealed class CreditWorthinessHouseholdService
{
    public async Task<List<_Rip.CreditWorthinessHousehold>> CreateHouseholds(int salesArrangementId, CancellationToken cancellationToken)
    {
        // seznam domacnosti na SA
        var households = ServiceCallResult.ResolveAndThrowIfError<List<_SA.Household>>(await _householdService.GetHouseholdList(salesArrangementId, cancellationToken));
        if (!households.Any())
            throw new CisValidationException("There is no household bound for this SA");

        return (await households.SelectAsync(async household =>
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
                    Customers = new()
                };

                // clients
                if (household.CustomerOnSAId1.HasValue)
                    h.Customers.Add(await createClient(household.CustomerOnSAId1.Value, cancellationToken));
                if (household.CustomerOnSAId2.HasValue)
                    h.Customers.Add(await createClient(household.CustomerOnSAId2.Value, cancellationToken));

                // Upravit validaci na FE API tak, aby hlídala, že aspoň jeden žadatel v každé z domácností na SA má vyplněný aspoň jeden příjem (=tedy nevalidovat, že každý žadatel musí mít vyplněný příjem)
                if (!h.Customers.Any(t => t.Incomes?.Any() ?? false))
                    throw new CisValidationException("At least one customer in household must have some income");

                return h;
            })).ToList();
    }

    private async Task<_Rip.CreditWorthinessCustomer> createClient(int customerOnSAId, CancellationToken cancellationToken)
    {
        // customer on SA instance
        var customer = ServiceCallResult.ResolveAndThrowIfError<_SA.CustomerOnSA>(await _customerOnSaService.GetCustomer(customerOnSAId, cancellationToken));

        var c = new _Rip.CreditWorthinessCustomer
        {
            // TODO:
            //HasPartner = customer.HasPartner
        };

        // customer instance
        if (customer.CustomerIdentifiers is not null && customer.CustomerIdentifiers.Any())
        {
            var customerInstance = ServiceCallResult.ResolveAndThrowIfError<DomainServices.CustomerService.Contracts.CustomerResponse>(await _customerService.GetCustomerDetail(new DomainServices.CustomerService.Contracts.CustomerRequest
            {
                Identity = customer.CustomerIdentifiers.First()
            }, cancellationToken));
            c.MaritalStateId = customerInstance.NaturalPerson?.MaritalStatusStateId;
        }

        //TODO neni tu zadani jake ID posilat, tak beru prvni
        if (customer.CustomerIdentifiers?.Any() ?? false)
            c.IdMp = customer.CustomerIdentifiers.First().IdentityId.ToString(System.Globalization.CultureInfo.InvariantCulture);

        if (customer.Incomes?.Any() ?? false)
            c.Incomes = customer.Incomes.Select(t => new _Rip.CreditWorthinessIncome
            {
                Amount = ((decimal?)t.Sum).GetValueOrDefault(0),
                IncomeTypeId = t.IncomeTypeId
            }).ToList();

        if (customer.Obligations?.Any() ?? false)
            c.Obligations = customer.Obligations.Select(t => new _Rip.CreditWorthinessObligation
            {
                ObligationTypeId = t.ObligationTypeId.GetValueOrDefault(),
                Installment = t.InstallmentAmount,
                Limit = t.CreditCardLimit,
                InstallmentConsolidated = t.Correction?.InstallmentAmountCorrection,//asi?
                AmountConsolidated = t.Correction?.CreditCardLimitCorrection,//asi?
                IsObligationCreditorExternal = t.Creditor?.IsExternal.GetValueOrDefault() ?? false
            }).ToList();

        return c;
    }

    private readonly DomainServices.SalesArrangementService.Abstraction.IHouseholdServiceAbstraction _householdService;
    private readonly DomainServices.SalesArrangementService.Abstraction.ICustomerOnSAServiceAbstraction _customerOnSaService;
    private readonly DomainServices.CustomerService.Abstraction.ICustomerServiceAbstraction _customerService;

    public CreditWorthinessHouseholdService(
        DomainServices.SalesArrangementService.Abstraction.IHouseholdServiceAbstraction householdService,
        DomainServices.CustomerService.Abstraction.ICustomerServiceAbstraction customerService,
        DomainServices.SalesArrangementService.Abstraction.ICustomerOnSAServiceAbstraction customerOnSaService)
    {
        _householdService = householdService;
        _customerService = customerService;
        _customerOnSaService = customerOnSaService;
    }
}
