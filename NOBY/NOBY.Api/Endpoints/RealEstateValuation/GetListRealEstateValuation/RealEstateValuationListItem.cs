using DomainServices.RealEstateValuationService.Contracts;
using System.ComponentModel.DataAnnotations;

namespace NOBY.Api.Endpoints.RealEstateValuation.GetListRealEstateValuation;

/// <summary>
/// Objednávka ocenění: přehledové údaje
/// </summary>
public sealed class RealEstateValuationListItem
{
    /// <summary>
    /// ID Ocenění nemovitosti v Noby
    /// </summary>
    /// <example>3456</example>
    public int? RealEstateValuationId { get; set; }

    /// <summary>
    /// Číslo objednávky ocenění
    /// </summary>
    /// <example>123455</example>
    public int? OrderId { get; set; }

    /// <summary>
    /// ID obchodního případu (caseId)
    /// </summary>
    /// <example>304556</example>
    [Required]
    public long CaseId { get; set; }

    /// <summary>
    /// ID typu nemovitosti
    /// </summary>
    /// <example>1</example>
    [Required]
    public int RealEstateTypeId { get; set; }

    /// <summary>
    /// Id ikony typu nemovitosti. 1 - house, 2 - location city, 3 - custom, 4 - domain
    /// </summary>
    [Required]
    public RealEstateTypeIcons RealEstateTypeIcon { get; set; }

    /// <summary>
    /// ID stavu Ocenění nemovitosti
    /// </summary>
    /// <example>8</example>
    [Required]
    public int ValuationStateId { get; set; }

    /// <summary>
    /// Indikátor stavu Ocenění nemovitosti, 0 - Unknown, 1 - Active, 2 - Cancelled, 3 - OK, 4 - Passive, 5 - Warning, 6 - Initial
    /// </summary>
    [Required]
    public int ValuationStateIndicator { get; set; }

    /// <summary>
    /// Název stavu Ocenění nemovitosti
    /// </summary>
    [Required]
    public string ValuationStateName { get; set; }

    /// <summary>
    /// True pokud jde o nemovitost, která je objektem úvěru
    /// </summary>
    [Required]
    public bool IsLoanRealEstate { get; set; }

    /// <summary>
    /// ID stavu nemovitosti. 0 - Unknown, 1 - Dokončená, 2 - V rekonstrukci, 3 - Projekt, 4 - Výstavba
    /// </summary>
    [Required]
    public int RealEstateStateId { get; set; }

    /// <summary>
    /// Název typu Ocenění nemovitosti. 0 - Unknown, 1 - Online, 2 - DTS, 3 - Standard
    /// </summary>
    [Required]
    public ValuationTypes ValuationTypeId { get; set; }

    /// <summary>
    /// Adresa nemovitosti
    /// </summary>
    /// <example>Novákovo nábřeží 1972, 521 71 Žďár nad Sázavou</example>
    public string? Address { get; set; }

    /// <summary>
    /// Datum odeslání žádosti o Ocenění nemovitosti
    /// </summary>
    public DateTime? ValuationSentDate { get; set; }

    /// <summary>
    /// Současná cena nemovitosti v Kč
    /// </summary>
    /// <example>3000000</example>
    public int? ValuationResultCurrentPrice { get; set; }

    /// <summary>
    /// Budoucí cena nemovitosti v Kč
    /// </summary>
    /// <example>3250000</example>
    public int? ValuationResultFuturePrice { get; set; }

    /// <summary>
    /// True pokud je potřeba kontrolní ocenění
    /// </summary>
    public bool? IsRevaluationRequired { get; set; }

    /// <summary>
    /// Informace o tom, zda byl na modelaci použit developer a zda je možné využít hromadné ocenění.
    /// </summary>
    [Required]
    public bool DeveloperAllowed { get; set; }

    /// <summary>
    /// True pokud je aplikované hromadné ocenění z developerského projektu
    /// </summary>
    [Required]
    public bool DeveloperApplied { get; set; }
}
