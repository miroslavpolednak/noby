using DomainServices.SalesArrangementService.Abstraction;
using _SA = DomainServices.SalesArrangementService.Contracts;

namespace FOMS.Api.Endpoints.CustomerObligation.GetObligation;

internal class GetObligationHandler
    : IRequestHandler<GetObligationRequest, Dto.ObligationFullDto>
{
    public async Task<Dto.ObligationFullDto> Handle(GetObligationRequest request, CancellationToken cancellationToken)
    {
        var obligationInstance = ServiceCallResult.ResolveAndThrowIfError<_SA.Obligation>(await _customerService.GetObligation(request.ObligationId, cancellationToken));

        return obligationInstance.ToApiResponse();
    }

    private readonly ICustomerOnSAServiceAbstraction _customerService;
    
    public GetObligationHandler(ICustomerOnSAServiceAbstraction customerService)
    {
        _customerService = customerService;
    }
}
