namespace NOBY.ApiContracts;

public partial class RealEstateValuationAddDeedOfOwnershipDocumentRequest
    : IRequest<int>
{
    [JsonIgnore]
    public long CaseId;

    [JsonIgnore]
    public int RealEstateValuationId;

    public RealEstateValuationAddDeedOfOwnershipDocumentRequest InfuseId(long caseId, int realEstateValuationId)
    {
        CaseId = caseId;
        RealEstateValuationId = realEstateValuationId;
        return this;
    }
}
