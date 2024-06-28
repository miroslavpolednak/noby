namespace NOBY.ApiContracts;

public partial class RealEstateValuationUpdateDeedOfOwnershipDocumentRequest
    : IRequest
{
    [JsonIgnore]
    public long CaseId;

    [JsonIgnore]
    public int RealEstateValuationId;

    [JsonIgnore]
    public int DeedOfOwnershipDocumentId;

    public RealEstateValuationUpdateDeedOfOwnershipDocumentRequest InfuseId(long caseId, int realEstateValuationId, int deedOfOwnershipDocumentId)
    {
        CaseId = caseId;
        RealEstateValuationId = realEstateValuationId;
        DeedOfOwnershipDocumentId = deedOfOwnershipDocumentId;

        return this;
    }
}
