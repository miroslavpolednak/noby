using System.Text.Json.Serialization;

namespace NOBY.Api.Endpoints.Refinancing.SendMortgageResponseCode;

public sealed class SendMortgageResponseCodeRequest
    : IRequest
{
    [JsonIgnore]
    internal long CaseId { get; set; }

    public int ResponseCodeTypeId { get; set; }

    public DateTime? DataDateTime { get; set; }

    public string? DataBankCode { get; set; }

    public string? DataString { get; set; }

    internal SendMortgageResponseCodeRequest InfuseId(long caseId)
    {
        this.CaseId = caseId;
        return this;
    }
}
