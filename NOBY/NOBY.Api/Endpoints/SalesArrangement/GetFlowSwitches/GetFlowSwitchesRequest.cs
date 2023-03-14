using System.Text.Json.Serialization;

namespace NOBY.Api.Endpoints.SalesArrangement.GetFlowSwitches;

public sealed class GetFlowSwitchesRequest
    : IRequest<GetFlowSwitchesResponse>
{
    [JsonIgnore]
    internal int SalesArrangementId;

    internal GetFlowSwitchesRequest InfuseId(int salesArrangementId)
    {
        this.SalesArrangementId = salesArrangementId;
        return this;
    }
}
