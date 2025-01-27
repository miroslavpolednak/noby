﻿using DomainServices.HouseholdService.Contracts;

namespace NOBY.Api.Endpoints.Household.UpdateCustomers;

internal static class Extensions
{
    public static CustomerOnSABase ToDomainServiceRequest(this HouseholdCustomer customer, CustomerOnSA? currentCustomer = null)
    {
        var model = new CustomerOnSABase
        {
            DateOfBirthNaturalPerson = customer.DateOfBirth,
            FirstNameNaturalPerson = customer.FirstName ?? "",
            Name = customer.LastName ?? "",
            MaritalStatusId = currentCustomer?.MaritalStatusId
        };

        if (customer.LockedIncome)
            model.LockedIncomeDateTime = currentCustomer?.LockedIncomeDateTime is null ? DateTime.Now : currentCustomer.LockedIncomeDateTime;

        if (customer.Identity is not null)
            model.CustomerIdentifiers.Add(new SharedTypes.GrpcTypes.Identity(customer.Identity));

        return model;
    }
}
