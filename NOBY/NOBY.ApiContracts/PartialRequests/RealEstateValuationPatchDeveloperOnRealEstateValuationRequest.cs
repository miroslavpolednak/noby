namespace NOBY.ApiContracts;

public partial class RealEstateValuationPatchDeveloperOnRealEstateValuationRequest
    : IRequest
{
    [JsonIgnore]
    public long CaseId { get; private set; }

    [JsonIgnore]
    public int RealEstateValuationId { get; private set; }

    public RealEstateValuationPatchDeveloperOnRealEstateValuationRequest InfuseId(long caseId, int realEstateValuationId)
    {
        this.RealEstateValuationId = realEstateValuationId;
        this.CaseId = caseId;
        return this;
    }
}
