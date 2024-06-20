using DomainServices.HouseholdService.Clients;

namespace NOBY.Api.Endpoints.CustomerObligation.GetObligation;

internal sealed class GetObligationHandler
    : IRequestHandler<GetObligationRequest, SharedDto.ObligationFullDto>
{
    public async Task<SharedDto.ObligationFullDto> Handle(GetObligationRequest request, CancellationToken cancellationToken)
    {
        var obligationInstance = await _customerService.GetObligation(request.ObligationId, cancellationToken);

        return obligationInstance.ToApiResponse();
    }

    private readonly ICustomerOnSAServiceClient _customerService;
    
    public GetObligationHandler(ICustomerOnSAServiceClient customerService)
    {
        _customerService = customerService;
    }
}
