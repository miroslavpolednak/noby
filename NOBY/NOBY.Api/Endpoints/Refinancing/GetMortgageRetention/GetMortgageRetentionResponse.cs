namespace NOBY.Api.Endpoints.Refinancing.GetMortgageRetention;

public sealed class GetMortgageRetentionResponse
{
    /// <summary>
    /// Informace zda se jedná o readonly režim
    /// </summary>
    public bool IsReadOnly { get; set; }

    public decimal FeeAmount { get; set; }

    /// <summary>
    /// Upravená výše poplatku
    /// </summary>
    public decimal? FeeAmountDiscounted { get; set; }

    /// <summary>
    /// Platnost nové úrokové sazby od.
    /// </summary>
    public DateTime InterestRateValidFrom { get; set; }

    /// <summary>
    /// Úroková sazba
    /// </summary>
    /// <example>4.7</example>
    public decimal InterestRate { get; set; }

    /// <summary>
    /// Sleva ze sazby
    /// </summary>
    /// <example>0.2</example>
    public decimal? InterestRateDiscount { get; set; }

    /// <summary>
    /// Sleva ze sazby
    /// </summary>
    /// <example>4.5</example>
    /// !!! dopocitat pro FE - rozdil mezi InterestRate - InterestRateDiscounted
    public decimal? InterestRateDiscounted { get => InterestRateDiscount.HasValue ? InterestRate - InterestRateDiscount : null; }

    /// <summary>
    /// Výše měsíční splátky.
    /// </summary>
    public decimal LoanPaymentAmount { get; set; }

    /// <summary>
    /// Výše měsíční splátky se zohledněním IC.
    /// </summary>
    public decimal? LoanPaymentAmountDiscounted { get; set; }

    /// <summary>
    /// Příznak nastavený na true pokud jsou data o IC v nalinkované simulaci rozdílné od aktuální IC v SB
    /// </summary>
    public bool ContainsInconsistentIndividualPriceData { get; set; }

    /// <summary>
    /// Komentář k IC
    /// </summary>
    public string? IndividualPriceCommentLastVersion { get; set; }

    public string? DocumentId { get; set; }

    public int? RefinancingDocumentEACode { get; set; }

    public int? SignatureTypeDetailId { get; set; }

    /// <summary>
    /// Označuje zda má být aktivní button Generovat dokument
    /// </summary>
    // offerid is not null, refinancingState=1, dalsi podminka je prvni pinda pokud existuje offer
    public bool IsGenerateDocumentEnabled { get; set; }

    /// <summary>
    /// Všechny workflow úkoly pro daný proces retence
    /// </summary>
    public List<NOBY.Dto.Workflow.WorkflowTask>? Tasks { get; set; }
}