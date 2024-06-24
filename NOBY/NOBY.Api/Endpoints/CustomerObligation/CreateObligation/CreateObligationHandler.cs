using DomainServices.HouseholdService.Clients;
using _HO = DomainServices.HouseholdService.Contracts;

namespace NOBY.Api.Endpoints.CustomerObligation.CreateObligation;

internal sealed class CreateObligationHandler(ICustomerOnSAServiceClient _customerService)
    : IRequestHandler<CustomerObligationCreateObligationRequest, int>
{
    public async Task<int> Handle(CustomerObligationCreateObligationRequest request, CancellationToken cancellationToken)
    {
        var model = new _HO.CreateObligationRequest
        {
            CustomerOnSAId = request.CustomerOnSAId!.Value,
            ObligationTypeId = request.ObligationTypeId!.Value,
            ObligationState = request.ObligationState,
            InstallmentAmount = request.InstallmentAmount,
            LoanPrincipalAmount = request.LoanPrincipalAmount,
            AmountConsolidated = request.AmountConsolidated,
            CreditCardLimit = request.CreditCardLimit,
            Creditor = request.Creditor?.ToDomainServiceRequest(),
            Correction = request.Correction?.ToDomainServiceRequest()
        };

        return await _customerService.CreateObligation(model, cancellationToken);
    }
}
