using CIS.Foms.Types;
using Google.Protobuf.Collections;
using contracts = DomainServices.SalesArrangementService.Contracts;

namespace FOMS.Api.Endpoints.Household.SaveHouseholds;

[CIS.Infrastructure.Attributes.ScopedService, CIS.Infrastructure.Attributes.SelfService]
internal class Mapper
{
    public contracts.CreateCustomerRequest MapToRequest(int salesArrangementId, Dto.CustomerInHousehold model)
    {
        var result = new contracts.CreateCustomerRequest()
        {
            SalesArrangementId = salesArrangementId,
            FirstNameNaturalPerson = model.FirstName,
            Name = model.LastName,
            DateOfBirthNaturalPerson = model.DateOfBirth,
            CustomerRoleId = model.RoleId,
        };
        
        model.Identities?.ForEach(t => result.CustomerIdentifiers.Add(new CIS.Infrastructure.gRPC.CisTypes.Identity(t.Id, t.Scheme)));
        
        return result;
    }
    
    public contracts.UpdateCustomerRequest MapToRequest(Dto.CustomerInHousehold model)
    {
        var result = new contracts.UpdateCustomerRequest()
        {
            FirstNameNaturalPerson = model.FirstName,
            Name = model.LastName,
            DateOfBirthNaturalPerson = model.DateOfBirth,
            CustomerRoleId = model.RoleId
        };
        
        model.Identities?.ForEach(t => result.CustomerIdentifiers.Add(new CIS.Infrastructure.gRPC.CisTypes.Identity(t.Id, t.Scheme)));
        
        return result;
    }
    
    public contracts.CreateHouseholdRequest MapToRequest(int salesArrangementId, Dto.Household household)
        => new()
        {
            HouseholdTypeId = household.HouseholdTypeId,
            SalesArrangementId = salesArrangementId,
            Expenses = mapExpenses(household.Expenses),
            Data = mapData(household.Data)
        };
    
    public contracts.UpdateHouseholdRequest MapToRequest(Dto.Household household)
        => new()
        {
            Expenses = mapExpenses(household.Expenses),
            Data = mapData(household.Data)
        };
    
    private contracts.HouseholdData? mapData(Dto.HouseholdData? model)
    {
        if (model is null) return null;
        return new contracts.HouseholdData()
        {
            ChildrenOverTenYearsCount = model.ChildrenOverTenYearsCount,
            ChildrenUpToTenYearsCount = model.ChildrenUpToTenYearsCount,
            PropertySettlementId = model.PropertySettlementId
        };
    }

    private contracts.Expenses? mapExpenses(Dto.HouseholdExpenses? model)
    {
        if (model is null) return null;
        return new contracts.Expenses()
        {
            HousingExpenseAmount = model.HousingExpenseAmount,
            InsuranceExpenseAmount = model.InsuranceExpenseAmount,
            SavingExpenseAmount = model.SavingExpenseAmount,
            OtherExpenseAmount = model.OtherExpenseAmount
        };
    }
}