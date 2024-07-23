namespace NOBY.ApiContracts;

public partial class SalesArrangementUpdateParametersRequest
    : IRequest
{
    [JsonIgnore]
    public int SalesArrangementId { get; private set; }

    public SalesArrangementUpdateParametersRequest InfuseId(int salesArrangementId)
    {
        SalesArrangementId = salesArrangementId;
        return this;
    }
}
