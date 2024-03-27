namespace NOBY.Api.Endpoints.Refinancing.GetRetentionDetail;

public sealed class GetRetentionDetailResponse
{
    /// <summary>
    /// Informace zda se jedná o readonly režim
    /// </summary>
    public bool IsReadonly { get; set; }

    /// <summary>
    /// Příznak nastavený na true pokud jsou data o IC v nalinkované simulaci rozdílné od aktuální IC v SB
    /// </summary>
    public bool ContainsInconsistentIndividualPriceData { get; set; }

    public string? ICkomentar { get; set; }

    /// <summary>
    /// Aktuálně vybraná nabídka
    /// </summary>
    public Dto.GetRetentionDetailOffer? Offer { get; set; }

    /// <summary>
    /// Informace o aktuální cenové vyjímce pokud existuje
    /// </summary>
    public Dto.GetRetentionDetailPriceExceptionTask? CurrentPriceExceptionTask { get; set; }

    /// <summary>
    /// Všechny workflow úkoly pro daný proces retence
    /// </summary>
    public List<NOBY.Dto.Workflow.WorkflowTask>? OtherWorkflowTasks { get; set; }
}