namespace NOBY.Api.Endpoints.Customer.GetDetailWithChanges;

internal sealed class GetDetailWithChangesHandler
    : IRequestHandler<GetDetailWithChangesRequest, GetDetailWithChangesResponse>
{
    public async Task<GetDetailWithChangesResponse> Handle(GetDetailWithChangesRequest request, CancellationToken cancellationToken)
    {
        return new GetDetailWithChangesResponse();
    }

    private readonly DomainServices.HouseholdService.Clients.ICustomerOnSAServiceClient _customerOnSAService;

    public GetDetailWithChangesHandler(
        DomainServices.HouseholdService.Clients.ICustomerOnSAServiceClient customerOnSAService)
    {
        _customerOnSAService = customerOnSAService;
    }
}
