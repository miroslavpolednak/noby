namespace NOBY.ApiContracts;

public partial class RealEstateValuationSaveLocalSurveyDetailsRequest
    : IRequest
{
    [JsonIgnore]
    public long CaseId { get; private set; }

    [JsonIgnore]
    public int RealEstateValuationId { get; private set; }

    public RealEstateValuationSaveLocalSurveyDetailsRequest InfuseId(long caseId, int realEstateValuationId)
    {
        CaseId = caseId;
        RealEstateValuationId = realEstateValuationId;
        return this;
    }
}
