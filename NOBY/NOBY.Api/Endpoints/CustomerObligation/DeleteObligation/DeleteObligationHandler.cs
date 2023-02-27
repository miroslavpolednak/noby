using DomainServices.HouseholdService.Clients;

namespace NOBY.Api.Endpoints.CustomerObligation.DeleteObligation;

internal class DeleteObligationHandler
    : IRequestHandler<DeleteObligationRequest>
{
    public async Task Handle(DeleteObligationRequest request, CancellationToken cancellationToken)
    {
        await _customerService.DeleteObligation(request.ObligationId, cancellationToken);
    }

    private readonly ICustomerOnSAServiceClient _customerService;
    
    public DeleteObligationHandler(ICustomerOnSAServiceClient customerService)
    {
        _customerService = customerService;
    }
}
