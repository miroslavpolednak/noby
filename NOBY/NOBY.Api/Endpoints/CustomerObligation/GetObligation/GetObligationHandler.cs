using DomainServices.HouseholdService.Clients.v1;

namespace NOBY.Api.Endpoints.CustomerObligation.GetObligation;

internal sealed class GetObligationHandler(ICustomerOnSAServiceClient _customerService)
    : IRequestHandler<GetObligationRequest, CustomerObligationObligationFull>
{
    public async Task<CustomerObligationObligationFull> Handle(GetObligationRequest request, CancellationToken cancellationToken)
    {
        var obligationInstance = await _customerService.GetObligation(request.ObligationId, cancellationToken);

        return obligationInstance.ToApiResponse();
    }
}
