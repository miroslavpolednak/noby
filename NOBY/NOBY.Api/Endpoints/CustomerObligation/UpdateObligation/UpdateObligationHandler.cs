using DomainServices.HouseholdService.Clients;
using _HO = DomainServices.HouseholdService.Contracts;

namespace NOBY.Api.Endpoints.CustomerObligation.UpdateObligation;

internal sealed class UpdateObligationHandler(ICustomerOnSAServiceClient _customerService)
    : IRequestHandler<CustomerObligationUpdateObligationRequest>
{
    public async Task Handle(CustomerObligationUpdateObligationRequest request, CancellationToken cancellationToken)
    {
        var obligationInstance = await _customerService.GetObligation(request.ObligationId, cancellationToken);

        var model = new _HO.Obligation
        {
            ObligationId = request.ObligationId,
            CustomerOnSAId = obligationInstance.CustomerOnSAId,
            ObligationState = request.ObligationState,
            CreditCardLimit = request.CreditCardLimit,
            InstallmentAmount = request.InstallmentAmount,
            ObligationTypeId = request.ObligationTypeId,
            LoanPrincipalAmount = request.LoanPrincipalAmount,
            AmountConsolidated = request.AmountConsolidated,
            Creditor = request.Creditor?.ToDomainServiceRequest(),
            Correction = request.Correction?.ToDomainServiceRequest()
        };

        await _customerService.UpdateObligation(model, cancellationToken);
    }
}
