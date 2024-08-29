using DomainServices.HouseholdService.Clients.v1;

namespace NOBY.Api.Endpoints.CustomerObligation.DeleteObligation;

internal sealed class DeleteObligationHandler(ICustomerOnSAServiceClient _customerService)
    : IRequestHandler<DeleteObligationRequest>
{
    public async Task Handle(DeleteObligationRequest request, CancellationToken cancellationToken)
    {
        await _customerService.DeleteObligation(request.ObligationId, cancellationToken);
    }
}
