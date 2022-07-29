using DomainServices.SalesArrangementService.Abstraction;

namespace FOMS.Api.Endpoints.CustomerObligation.DeleteObligation;

internal class DeleteObligationHandler
    : AsyncRequestHandler<DeleteObligationRequest>
{
    protected override async Task Handle(DeleteObligationRequest request, CancellationToken cancellationToken)
    {
        ServiceCallResult.Resolve(await _customerService.DeleteObligation(request.ObligationId, cancellationToken));
    }

    private readonly ICustomerOnSAServiceAbstraction _customerService;
    
    public DeleteObligationHandler(ICustomerOnSAServiceAbstraction customerService)
    {
        _customerService = customerService;
    }
}
