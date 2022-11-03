namespace NOBY.Api.Endpoints.SalesArrangement.GetLoanApplicationAssessment.Dto;

public class AssessmentReason
{
    /// <summary>
    /// Kód důvodu
    /// </summary>
    public string? Code { get; set; }

    /// <summary>
    /// Úroveň důvodu
    /// </summary>
    public string? Level { get; set; }

    /// <summary>
    /// Váha důvodu
    /// </summary>
    public long? Weight { get; set; }

    /// <summary>
    /// Cíl důvodu
    /// </summary>
    public string? Target { get; set; }

    /// <summary>
    /// Popis důvodu
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Výsledek důvodu
    /// </summary>
    public string? Result { get; set; }
}
