using SharedTypes.Enums;

namespace NOBY.Dto.Refinancing;

public sealed class ProcessDetail
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
    public RefinancingStates RefinancingStateId { get; set; }

    /// <summary>
    /// Datum vzniku proces
    /// </summary>
    public DateOnly CreatedOn { get; set; }

    /// <summary>
    /// Úroková sazba poskytnutá (včetně slevy)
    /// </summary>
    public decimal? LoanInterestRateProvided { get; set; }

    /// <summary>
    /// Platnost nové úrokové sazby od
    /// </summary>
    public DateOnly? LoanInterestRateValidFrom { get; set; }

    /// <summary>
    /// Platnost nové úrokové sazby do
    /// </summary>
    public DateOnly? LoanInterestRateValidTo { get; set; }

    /// <summary>
    /// Datum účinnosti (kdy byl požadavek podepsán a finalizován ve SB)
    /// </summary>
    public DateOnly? EffectiveDate { get; set; }

    /// <summary>
    /// Slouží k náhledu na dokumenty
    /// </summary>
    public string? DocumentId { get; set; }

    /// <summary>
    /// Název stavu procesu Refinancí
    /// </summary>
    public string RefinancingStateName { get; set; } = null!;

    /// <summary>
    /// Indikátor barvy stavu
    /// </summary>
    public int RefinancingStateIndicator { get; set; }
}
