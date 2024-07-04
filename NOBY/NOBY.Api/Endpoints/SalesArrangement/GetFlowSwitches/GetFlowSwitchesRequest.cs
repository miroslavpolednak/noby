namespace NOBY.Api.Endpoints.SalesArrangement.GetFlowSwitches;

internal sealed record GetFlowSwitchesRequest(int SalesArrangementId)
    : IRequest<SalesArrangementGetFlowSwitchesResponse>
{
}
