namespace NOBY.Api.Endpoints.Cases.GetCaseParameters;

public sealed class GetCaseParametersResponse
{
    public List<Dto.CaseParameters> CaseParameters { get; set; } = null!;

    public Dto.SalesArrangementInProgressDto SalesArrangementInProgress { get; set; } = null!;
}
