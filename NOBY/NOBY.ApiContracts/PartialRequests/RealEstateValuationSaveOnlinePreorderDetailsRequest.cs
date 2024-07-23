namespace NOBY.ApiContracts;

public partial class RealEstateValuationSaveOnlinePreorderDetailsRequest
    : IRequest
{
    [JsonIgnore]
    public long CaseId { get; private set; }

    [JsonIgnore]
    public int RealEstateValuationId { get; private set; }

    public RealEstateValuationSaveOnlinePreorderDetailsRequest InfuseId(long caseId, int realEstateValuationId)
    {
        CaseId = caseId;
        RealEstateValuationId = realEstateValuationId;
        return this;
    }
}
