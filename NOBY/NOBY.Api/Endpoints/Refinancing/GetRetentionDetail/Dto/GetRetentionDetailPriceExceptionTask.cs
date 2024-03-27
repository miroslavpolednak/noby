namespace NOBY.Api.Endpoints.Refinancing.GetRetentionDetail.Dto;

public sealed class GetRetentionDetailPriceExceptionTask
{
    /// <summary>
    /// Detail úkolu
    /// </summary>
    public NOBY.Dto.Workflow.WorkflowTask Task { get; set; } = null!;

    /// <summary>
    /// Detail cenové vyjímky
    /// </summary>
    public NOBY.Dto.Workflow.AmendmentsPriceException? PriceExceptionDetails { get; set; } = null!;
}
