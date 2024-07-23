namespace NOBY.ApiContracts;

public partial class RefinancingCreateMortgageResponseCodeRequest
    : IRequest
{
    [JsonIgnore]
    public long CaseId { get; private set; }

    public RefinancingCreateMortgageResponseCodeRequest InfuseId(long caseId)
    {
        this.CaseId = caseId;
        return this;
    }
}
