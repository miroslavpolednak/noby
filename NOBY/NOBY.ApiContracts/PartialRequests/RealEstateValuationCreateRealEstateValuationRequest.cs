namespace NOBY.ApiContracts;

public partial class RealEstateValuationCreateRealEstateValuationRequest
    : IRequest<int>
{
    [JsonIgnore]
    public long CaseId { get; private set; }

    public RealEstateValuationCreateRealEstateValuationRequest InfuseId(long caseId)
    {
        this.CaseId = caseId;
        return this;
    }
}
