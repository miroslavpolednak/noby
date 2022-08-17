namespace FOMS.Api.Endpoints.CustomerObligation.Dto;

public class ObligationCorrectionDto
{
    /// <summary>
    /// Z číselníku <a href="https://wiki.kb.cz/pages/viewpage.action?pageId=440890324">ObligationCorrectionType (CIS_KOREKCE_ZAVAZKU)</a>
    /// </summary>
    public int? CorrectionTypeId { get; set; }

    /// <summary>
    /// Výše konsolidace/zrušení splátky daného závazku
    /// </summary>
    public decimal? InstallmentAmountCorrection { get; set; }

    /// <summary>
    /// Výše konsolidace/zrušení/snížení výše úvěru
    /// </summary>
    public decimal? LoanPrincipalAmountCorrection { get; set; }

    /// <summary>
    /// Výše konsolidace/zrušení/snížení limitu KK/PD
    /// </summary>
    public decimal? CreditCardLimitCorrection { get; set; }
}
