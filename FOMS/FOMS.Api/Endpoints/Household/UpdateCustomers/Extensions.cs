using DomainServices.SalesArrangementService.Contracts;

namespace FOMS.Api.Endpoints.Household.UpdateCustomers;

internal static class Extensions
{
    public static List<CustomerObligation> ToDomainServiceRequest(this List<Dto.CustomerObligation> obligations)
        => obligations.Select(t => new CustomerObligation
        {
            CreditCardLimit = t.CreditCardLimit,
            CreditCardLimitConsolidated = t.CreditCardLimitConsolidated,
            IsObligationCreditorExternal = t.IsObligationCreditorExternal,
            ObligationTypeId = t.ObligationTypeId,
            LoanPrincipalAmount = t.LoanPrincipalAmount,
            LoanPrincipalAmountConsolidated = t.LoanPrincipalAmountConsolidated,
            InstallmentAmount = t.InstallmentAmount,
            InstallmentAmountConsolidated = t.InstallmentAmountConsolidated,
            ObligationState = 1
        })
        .ToList();

    public static CustomerOnSABase ToDomainServiceRequest(this CustomerDto customer)
    {
        var model = new CustomerOnSABase
        {
            DateOfBirthNaturalPerson = customer.DateOfBirth,
            FirstNameNaturalPerson = customer.FirstName,
            Name = customer.LastName,
        };
        if (customer.Identity is not null)
            model.CustomerIdentifiers.Add(new CIS.Infrastructure.gRPC.CisTypes.Identity(customer.Identity));
        return model;
    }

    public static List<Dto.CustomerObligation>? CastToCustomerObligations(this List<Dto.HouseholdCustomerObligation>? list, int index)
        => list?.Where(t => t.CustomerIndex == index)
        .Select(t => new Dto.CustomerObligation
        {
            CreditCardLimit = t.CreditCardLimit,
            CreditCardLimitConsolidated = t.CreditCardLimitConsolidated,
            IsObligationCreditorExternal = t.IsObligationCreditorExternal,
            ObligationTypeId = t.ObligationTypeId,
            LoanPrincipalAmount = t.LoanPrincipalAmount,
            InstallmentAmount = t.InstallmentAmount,
            InstallmentAmountConsolidated = t.InstallmentAmountConsolidated,
            LoanPrincipalAmountConsolidated = t.LoanPrincipalAmountConsolidated
        }).ToList();
}
