using SharedTypes.Enums;

namespace NOBY.Dto.Refinancing;
public class ProcessDetail
{
    /// <summary>
    /// Noby proces ID daného procesu Refinancí. Jde o ID sady úkolů generované Starbuildem.
    /// </summary>
    public long ProcessId { get; set; }

    public RefinancingTypes RefinancingTypeId { get; set; }

    /// <summary>
    /// Vracíme textaci specifickou podle typu dodatku. Pro jeden refinancingTypeId se tedy může vrátit 1 z n různých textací.
    /// </summary>
    public string? RefinancingTypeText { get; set; }

    /// <summary>
    /// Číselník: <a href="https://wiki.kb.cz/display/HT/RefinancingState">RefinancingState</a>
    /// </summary>
    public int RefinancingStateId { get; set; }

    public DateTime CreatedTime { get; set; }

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
