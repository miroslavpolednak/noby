using contracts = DomainServices.SalesArrangementService.Contracts;

namespace FOMS.Api.Endpoints.Household.UpdateHousehold;

internal static class Extensions
{
    #region customer
    public static contracts.UpdateCustomerRequest MapToRequest(this Dto.CustomerInHousehold model)
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

    public static contracts.CreateCustomerRequest MapToRequest(this Dto.CustomerInHousehold model, int salesArrangementId)
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
    #endregion customer

    #region household
    public static contracts.HouseholdData? MapToRequest(this Dto.HouseholdData? model)
    {
        if (model is null) return null;
        return new contracts.HouseholdData()
        {
            ChildrenOverTenYearsCount = model.ChildrenOverTenYearsCount,
            ChildrenUpToTenYearsCount = model.ChildrenUpToTenYearsCount,
            PropertySettlementId = model.PropertySettlementId
        };
    }

    public static contracts.Expenses? MapToRequest(this Dto.HouseholdExpenses? model)
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
    #endregion household
}
