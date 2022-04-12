using DomainServices.SalesArrangementService.Contracts;

namespace FOMS.Api.Endpoints.Household.UpdateCustomers;

internal static class Extensions
{
    public static List<CustomerObligation> ToDomainServiceRequest(this List<Dto.CustomerObligation> obligations)
        => obligations.Select(t => new CustomerObligation
        {
            CreditCardLimit = t.CreditCardLimit,
            LoanPaymentAmount = t.LoanPaymentAmount,
            IsObligationCreditorExternal = t.IsObligationCreditorExternal,
            ObligationTypeId = t.ObligationTypeId,
            RemainingLoanPrincipal = t.RemainingLoanPrincipal,
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
}
