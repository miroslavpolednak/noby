using DomainServices.HouseholdService.Contracts;

namespace NOBY.Api.Endpoints.Household.UpdateCustomers;

internal static class Extensions
{
    public static CustomerOnSABase ToDomainServiceRequest(this CustomerDto customer, CIS.Infrastructure.gRPC.CisTypes.NullableGrpcDateTime? lockedIncomeDateTime = null)
    {
        var model = new CustomerOnSABase
        {
            DateOfBirthNaturalPerson = customer.DateOfBirth,
            FirstNameNaturalPerson = customer.FirstName ?? "",
            Name = customer.LastName ?? ""
        };

        if (customer.LockedIncome)
            model.LockedIncomeDateTime = lockedIncomeDateTime is null ? DateTime.Now : lockedIncomeDateTime;

        if (customer.Identity is not null)
            model.CustomerIdentifiers.Add(new CIS.Infrastructure.gRPC.CisTypes.Identity(customer.Identity));

        return model;
    }
}
