using System.Text.Json.Serialization;

namespace NOBY.Api.Endpoints.RealEstateValuation.SaveLocalSurveyDetails;

public sealed class SaveLocalSurveyDetailsRequest
    : Dto.RealEstateValuation.LocalSurveyData, IRequest
{
    [JsonIgnore]
    internal long CaseId;

    [JsonIgnore]
    internal int RealEstateValuationId;

    internal SaveLocalSurveyDetailsRequest InfuseId(long caseId, int realEstateValuationId)
    {
        CaseId = caseId;
        RealEstateValuationId = realEstateValuationId;

        return this;
    }
}
