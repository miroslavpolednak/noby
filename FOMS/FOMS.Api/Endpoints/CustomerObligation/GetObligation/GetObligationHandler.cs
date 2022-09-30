using DomainServices.HouseholdService.Clients;
using _HO = DomainServices.HouseholdService.Contracts;

namespace FOMS.Api.Endpoints.CustomerObligation.GetObligation;

internal class GetObligationHandler
    : IRequestHandler<GetObligationRequest, Dto.ObligationFullDto>
{
    public async Task<Dto.ObligationFullDto> Handle(GetObligationRequest request, CancellationToken cancellationToken)
    {
        var obligationInstance = ServiceCallResult.ResolveAndThrowIfError<_HO.Obligation>(await _customerService.GetObligation(request.ObligationId, cancellationToken));

        return obligationInstance.ToApiResponse();
    }

    private readonly ICustomerOnSAServiceClient _customerService;
    
    public GetObligationHandler(ICustomerOnSAServiceClient customerService)
    {
        _customerService = customerService;
    }
}
