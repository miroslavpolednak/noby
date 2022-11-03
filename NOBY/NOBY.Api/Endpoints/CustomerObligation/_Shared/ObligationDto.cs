namespace NOBY.Api.Endpoints.CustomerObligation.Dto;

public abstract class ObligationDto
{
    /// <summary>
    /// Druh závazku, z číselníku <a href="https://wiki.kb.cz/pages/viewpage.action?pageId=426150084">ObligationType (CIS_DRUH_ZAVAZKU)</a>
    /// </summary>
    public int? ObligationTypeId { get; set; }

    /// <summary>
    /// Výše splátky
    /// </summary>
    public decimal? InstallmentAmount { get; set; }

    /// <summary>
    /// Výše nesplacené jistiny
    /// </summary>
    public decimal? LoanPrincipalAmount { get; set; }

    /// <summary>
    /// Výše limitu kreditní karty
    /// </summary>
    public decimal? CreditCardLimit { get; set; }

    public decimal? AmountConsolidated { get; set; }

    /// <summary>
    /// Určuje stav závazku (prohlášený vs neprohlášený. V MPV bude vždy prohlášený.<br />Z číselníku <a href="https://wiki.kb.cz/display/HT/ObligationState">ObligationState</a>
    /// </summary>
    public int? ObligationState { get; set; }

    public ObligationCreditorDto? Creditor { get; set; }

    public ObligationCorrectionDto? Correction { get; set; }
}
