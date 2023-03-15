namespace NOBY.Api.Endpoints.SalesArrangement.GetFlowSwitches;

internal sealed class GetFlowSwitchesRequest
    : IRequest<GetFlowSwitchesResponse>
{
    internal int SalesArrangementId;

    public GetFlowSwitchesRequest(int salesArrangementId)
    {
        this.SalesArrangementId = salesArrangementId;
    }
}
