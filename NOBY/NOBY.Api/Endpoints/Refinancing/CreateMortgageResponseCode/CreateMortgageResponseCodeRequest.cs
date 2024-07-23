using System.Text.Json.Serialization;

namespace NOBY.Api.Endpoints.Refinancing.CreateMortgageResponseCode;

public sealed class CreateMortgageResponseCodeRequest
    : IRequest
{
    [JsonIgnore]
    internal long CaseId { get; set; }

    public int ResponseCodeTypeId { get; set; }

    public DateTime? DataDateTime { get; set; }

    public string? DataBankCode { get; set; }

    public string? DataString { get; set; }

    internal CreateMortgageResponseCodeRequest InfuseId(long caseId)
    {
        this.CaseId = caseId;
        return this;
    }
}
