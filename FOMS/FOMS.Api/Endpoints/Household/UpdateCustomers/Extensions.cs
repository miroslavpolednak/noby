using DomainServices.SalesArrangementService.Contracts;

namespace FOMS.Api.Endpoints.Household.UpdateCustomers;

internal static class Extensions
{
    public static CustomerOnSABase ToDomainServiceRequest(this CustomerDto customer, DateTime? lockedIncomeDateTime)
    {
        var model = new CustomerOnSABase
        {
            DateOfBirthNaturalPerson = customer.DateOfBirth,
            FirstNameNaturalPerson = customer.FirstName,
            Name = customer.LastName
        };
        
        if (lockedIncomeDateTime.HasValue)
            model.LockedIncomeDateTime = lockedIncomeDateTime;
        
        if (customer.Identity is not null)
            model.CustomerIdentifiers.Add(new CIS.Infrastructure.gRPC.CisTypes.Identity(customer.Identity));

        return model;
    }
}
