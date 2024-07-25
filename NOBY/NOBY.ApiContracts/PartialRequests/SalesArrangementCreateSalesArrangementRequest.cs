namespace NOBY.ApiContracts;

public partial class SalesArrangementCreateSalesArrangementRequest : IRequest<SalesArrangementCreateSalesArrangementResponse>
{
    [JsonIgnore]
    public long CaseId;

    public SalesArrangementCreateSalesArrangementRequest InfuseId(long caseId)
    {
        CaseId = caseId;
        return this;
    }
}