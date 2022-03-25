using DomainServices.SalesArrangementService.Contracts;

namespace FOMS.Api.Endpoints.Household.UpdateCustomers;

internal static class Extensions
{
    public static List<CustomerObligation> ToDomainServiceRequest(this List<Dto.CustomerObligation> obligations)
        => obligations.Select(t => new CustomerObligation
        {
            CreditCardLimit = t.CreditCardLimit,
            LoanPaymentAmount = t.LoanPaymentAmount,
            ObligationCreditor = (CIS.Infrastructure.gRPC.CisTypes.Mandants)(int)t.ObligationCreditor,
            ObligationTypeId = t.ObligationTypeId,
            RemainingLoanPrincipal = t.RemainingLoanPrincipal,
            ObligationState = 1
        })
        .ToList();
}
