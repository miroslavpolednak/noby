namespace NOBY.Api.Endpoints.SalesArrangement.GetFlowSwitches;

internal sealed class GetFlowSwitchesHandler
    : IRequestHandler<GetFlowSwitchesRequest, GetFlowSwitchesResponse>
{
    public async Task<GetFlowSwitchesResponse> Handle(GetFlowSwitchesRequest request, CancellationToken cancellationToken)
    {
        var existingSwitches = await _salesArrangementService.GetFlowSwitches(request.SalesArrangementId, cancellationToken);

    }

    private readonly DomainServices.SalesArrangementService.Clients.ISalesArrangementServiceClient _salesArrangementService;

    public GetFlowSwitchesHandler(DomainServices.SalesArrangementService.Clients.ISalesArrangementServiceClient arrangementServiceClient)
    {
        _salesArrangementService = arrangementServiceClient;
    }
}
