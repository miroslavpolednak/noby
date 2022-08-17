using DomainServices.SalesArrangementService.Contracts;

namespace FOMS.Api.Endpoints.Household.UpdateCustomers;

internal static class Extensions
{
    public static CustomerOnSABase ToDomainServiceRequest(this CustomerDto customer, CIS.Infrastructure.gRPC.CisTypes.NullableGrpcDateTime lockedIncomeDateTime)
    {
        CIS.Infrastructure.gRPC.CisTypes.NullableGrpcDateTime newD = lockedIncomeDateTime;
        if (customer.LockedIncome && newD == null)
            newD = DateTime.Now;

        var model = new CustomerOnSABase
        {
            DateOfBirthNaturalPerson = customer.DateOfBirth,
            FirstNameNaturalPerson = customer.FirstName,
            Name = customer.LastName,
            LockedIncomeDateTime = newD
        };
        
        if (customer.Identity is not null)
            model.CustomerIdentifiers.Add(new CIS.Infrastructure.gRPC.CisTypes.Identity(customer.Identity));

        return model;
    }
}
