namespace NOBY.Dto.Refinancing;

public class BaseRefinancingDetailResponse
{
    /// <summary>
    /// Komentář k IC
    /// </summary>
    public string? IndividualPriceCommentLastVersion { get; set; }

    public string? Comment { get; set; }

    /// <summary>
    /// Všechny workflow úkoly pro daný proces retence
    /// </summary>
    public List<NOBY.Dto.Workflow.WorkflowTask>? Tasks { get; set; }

    /// <summary>
    /// Příznak nastavený na true pokud jsou data o IC v nalinkované simulaci rozdílné od aktuální IC v SB
    /// </summary>
    public bool ContainsInconsistentIndividualPriceData { get; set; }

    /// <summary>
    /// Seznam odpovědních kódů
    /// </summary>
    public List<RefinancingResponseCode>? ResponseCodes { get; set; }
}
