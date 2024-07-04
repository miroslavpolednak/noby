namespace NOBY.ApiContracts;

public partial class SalesArrangementUpdateCommentRequest
    : IRequest
{
    [JsonIgnore]
    public int SalesArrangementId { get; private set; }

    public SalesArrangementUpdateCommentRequest InfuseId(int salesArrangementId)
    {
        SalesArrangementId = salesArrangementId;
        return this;
    }
}
