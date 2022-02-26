using DomainServices.CodebookService.Abstraction;
using DomainServices.SalesArrangementService.Contracts;
using FOMS.Api.Endpoints.Household.Dto;
using contracts = DomainServices.SalesArrangementService.Contracts;

namespace FOMS.Api.Endpoints.Household.GetHouseholds;

[CIS.Infrastructure.Attributes.ScopedService, CIS.Infrastructure.Attributes.SelfService]
internal sealed class Mapper
{
    private readonly ICodebookServiceAbstraction _codebookService;

    public Mapper(ICodebookServiceAbstraction codebookService)
    {
        _codebookService = codebookService;
    }

    public async Task<List<Dto.Household>> MapToResponse(List<contracts.Household>? households, List<CustomerOnSA>? customers)
    {
        List<Dto.Household> response = new();
        if (households is null) return response;

        foreach (var t in households)
        {
            // create household
            var household = new Dto.Household
            {
                Id = t.HouseholdId,
                HouseholdTypeId = t.HouseholdTypeId,
                HouseholdTypeName = (await _codebookService.HouseholdTypes()).First(x => x.Id == t.HouseholdTypeId).Name,
                Expenses = mapExpenses(t.Expenses),
                Data = mapData(t.Data),
                Customers = new List<CustomerInHousehold>()
            };
            
            // customers
            addCustomer(t.CustomerOnSAId1, household);
            addCustomer(t.CustomerOnSAId2, household);
            
            response.Add(household);
        }

        return response;
        
        void addCustomer(int? customerOnSAId, Dto.Household household)
        {
            if (customerOnSAId.HasValue && customers is not null)
            {
                var customer = mapCustomer(customers.FirstOrDefault(x => x.CustomerOnSAId == customerOnSAId));
                //TODO co delat, kdyz ho nenajdu? imo je to chyba
                if (customer is not null)
                    household.Customers!.Add(customer);
            }
        }
    }

    static Dto.CustomerInHousehold? mapCustomer(CustomerOnSA? model)
        => model is null
            ? null
            : new Dto.CustomerInHousehold()
            {
                Id = model.CustomerOnSAId,
                Identities = null,
                FirstName = model.FirstNameNaturalPerson,
                LastName = model.Name,
                DateOfBirth = model.DateOfBirthNaturalPerson,
                RoleId = model.CustomerRoleId
            };
    
    static Dto.HouseholdExpenses? mapExpenses(Expenses? model)
        => model is null
            ? null
            : new Dto.HouseholdExpenses()
            {
                InsuranceExpenseAmount = model.InsuranceExpenseAmount,
                SavingExpenseAmount = model.SavingExpenseAmount,
                HousingExpenseAmount = model.HousingExpenseAmount,
                OtherExpenseAmount = model.OtherExpenseAmount
            };

    static Dto.HouseholdData? mapData(contracts.HouseholdData? model)
        => model is null
            ? null
            : new Dto.HouseholdData()
            {
                PropertySettlementId = model.PropertySettlementId,
                ChildrenOverTenYearsCount = model.ChildrenOverTenYearsCount,
                ChildrenUpToTenYearsCount = model.ChildrenUpToTenYearsCount
            };
}