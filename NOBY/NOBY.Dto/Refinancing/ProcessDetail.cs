namespace NOBY.Dto.Refinancing;
public class ProcessDetail
{
    /// <summary>
    /// Noby proces ID daného procesu Refinancí. Jde o ID sady úkolů generované Starbuildem.
    /// </summary>
    public long ProcessId { get; set; }

    /// <summary>
    /// Enum: 0 - Unknown, 1 - Retence, 2 - Refixace
    /// </summary>
    public int RefinancingTypeId { get; set; }
    
    /// <summary>
    /// Vracíme textaci specifickou podle typu dodatku. Pro jeden refinancingTypeId se tedy může vrátit 1 z n různých textací.
    /// </summary>
    public string? RefinancingTypeText { get; set; }

    /// <summary>
    /// Hodnota z číselníku RefinancingState dynamicky vyhodnocovaná: RozpracovanoVNoby:1, RozpracovanoVSB:2, Podepisovani:3, Dokonceno:4, PredanoRC2:5, Zruseno:6, Unknow:0 
    /// </summary>
    public int RefinancingStateId { get; set; }

    public DateTime? CreatedTime { get; set; }

    /// <summary>
    /// Úroková sazba poskytnutá (včetně slevy)
    /// </summary>
    public decimal? LoanInterestRateProvided { get; set; }
    
    /// <summary>
    /// Platnost nové úrokové sazby od.
    /// </summary>
    public DateOnly? LoanInterestRateValidFrom { get; set; }

    /// <summary>
    /// Platnost nové úrokové sazby do (datum následující refixace).
    /// </summary>
    public DateTime? LoanInterestRateValidTo { get; set; }

    /// <summary>
    /// Datum účinnosti (kdy byl požadavek podepsán a finalizován ve SB)
    /// </summary>
    public DateTime? EffectiveDate { get; set; }

    /// <summary>
    /// Slouží k náhledu na dokumenty
    /// </summary>
    public string? DocumentId { get; set; }
}
