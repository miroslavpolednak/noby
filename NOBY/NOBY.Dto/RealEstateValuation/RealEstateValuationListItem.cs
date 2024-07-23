﻿using DomainServices.RealEstateValuationService.Contracts;
using SharedTypes.Enums;

namespace NOBY.Dto.RealEstateValuation;

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
    public long? OrderId { get; set; }

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
    /// <example>1</example>
    [Required]
    public int RealEstateTypeIcon { get; set; }

    /// <summary>
    /// ID stavu Ocenění nemovitosti
    /// </summary>
    /// <example>8</example>
    [Required]
    public int ValuationStateId { get; set; }

    /// <summary>
    /// Indikátor stavu Ocenění nemovitosti, 0 - Unknown, 1 - Active, 2 - Cancelled, 3 - OK, 4 - Passive, 5 - Warning, 6 - Initial
    /// </summary>
    /// <example>4</example>
    [Required]
    public ValuationStateIndicators ValuationStateIndicator { get; set; }

    /// <summary>
    /// Název stavu Ocenění nemovitosti
    /// </summary>
    /// <example>Probíhá ocenění</example>
    [Required]
    public string ValuationStateName { get; set; } = string.Empty;

    /// <summary>
    /// True pokud jde o nemovitost, která je objektem úvěru
    /// </summary>
    /// <example>true</example>
    [Required]
    public bool IsLoanRealEstate { get; set; }

    /// <summary>
    /// ID stavu nemovitosti. 1 - Dokončená, 2 - V rekonstrukci, 3 - Projekt, 4 - Výstavba
    /// </summary>
    /// <example>2</example>
    public int? RealEstateStateId { get; set; }

    /// <summary>
    /// Název typu Ocenění nemovitosti. 0 - Unknown, 1 - Online, 2 - DTS, 3 - Standard
    /// </summary>
    /// <example>DTS</example>
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
    /// <example>31.12.2023</example>
    public DateOnly? ValuationSentDate { get; set; }

    /// <summary>
    /// True pokud je potřeba kontrolní ocenění
    /// </summary>
    /// <example>false</example>
    public bool? IsRevaluationRequired { get; set; }

    /// <summary>
    /// Informace o tom, zda byl na modelaci použit developer a zda je možné využít hromadné ocenění.
    /// </summary>
    /// <example>true</example>
    [Required]
    public bool DeveloperAllowed { get; set; }

    /// <summary>
    /// True pokud je aplikované hromadné ocenění z developerského projektu
    /// </summary>
    /// <example>false</example>
    [Required]
    public bool DeveloperApplied { get; set; }

    /// <summary>
    /// Možné typy ocenění (výsledek ACV trychtýře), 1 - online, 2 - dts, 3 - standard
    /// </summary>
    public List<RealEstateValuationValuationTypes>? PossibleValuationTypeId { get; set; }

    public List<RealEstatePriceDetail>? Prices { get; set; }
}

//ITA chce enumy - me to neprijde dobre, fakticky se i na DS pouzivaji jen id, ale...
public enum ValuationStateIndicators
{
    Unknown = 0, 
    Active = 1, 
    Cancelled = 2, 
    OK = 3, 
    Passive = 4,
    Warning = 5,
    Initial = 6
}