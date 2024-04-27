namespace NOBY.Dto.Refinancing;

public sealed class ExtraPaymentSimulationResult
{
    /// <summary>
    /// Typ mimořádné splátky
    /// </summary>
    public bool IsExtraPaymentComplete { get; set; }

    /// <summary>
    /// Celková výše mimořádné splátky
    /// </summary>
    public decimal ExtraPaymentAmount { get; set; }

    /// <summary>
    /// Výše účelně vynaložených nákladů
    /// </summary>
    public decimal FeeAmount { get; set; }

    /// <summary>
    /// Jistina splacená mimořádnou splátkou
    /// </summary>
    public decimal PrincipalAmount { get; set; }

    /// <summary>
    /// Úroky splacené mimořádnou splátkou
    /// </summary>
    public decimal InterestAmount { get; set; }

    /// <summary>
    /// Neuhrazené poplatky splacené mimořádnou splátkou
    /// </summary>
    public decimal OtherUnpaidFees { get; set; }

    /// <summary>
    /// Úroky z prodlení splacené mimořádnou splátkou
    /// </summary>
    public decimal InterestOnLate { get; set; }

    /// <summary>
    /// Úroky s odloženou splatností splacené mimořádnou splátkou
    /// </summary>
    public decimal InterestCovid { get; set; }

    /// <summary>
    /// Příznak, zda je úvěr po splatnosti
    /// </summary>
    public bool IsLoanOverdue { get; set; }

    /// <summary>
    /// Příznak, zda se snižuje výše pravidelné splátky
    /// </summary>
    public bool IsPaymentReduced { get; set; }

    /// <summary>
    /// Nové datum splatnosti, pokud se zkracuje splacením mimořádné splátky
    /// </summary>
    public DateTime NewMaturityDate { get; set; }

    /// <summary>
    /// Nová výše splátky, pokud se snižuje výše splátky splacením mimořádné splátky
    /// </summary>
    public decimal NewPaymentAmount { get; set; }

    /// <summary>
    /// Typ pokuty za předčasné splacení
    /// </summary>
    public int FeeTypeId { get; set; }

    /// <summary>
    /// Základ pro výpočet pokuty
    /// </summary>
    public decimal FeeCalculationBase { get; set; }

    /// <summary>
    /// Smluvní pokuty
    /// </summary>
    public decimal FeeAmountContracted { get; set; }

    /// <summary>
    /// Období k fixaci od
    /// </summary>
    public DateTime FixedRateSanctionFreePeriodFrom { get; set; }

    /// <summary>
    /// Období k fixaci do
    /// </summary>
    public DateTime FixedRateSanctionFreePeriodTo { get; set; }

    /// <summary>
    /// Období bez sankce od
    /// </summary>
    public DateTime AnnualSanctionFreePeriodFrom { get; set; }

    /// <summary>
    /// Období bez sankce do
    /// </summary>
    public DateTime AnnualSanctionFreePeriodTo { get; set; }

    /// <summary>
    /// Částka k období bez sankce
    /// </summary>
    public decimal SanctionFreeAmount { get; set; }

    /// <summary>
    /// Spočítaná pokuta (Pokuta – sleva za pokutu, minimálně 0)
    /// </summary>
    public decimal FeeAmountTotal { get; set; }

    /// <summary>
    /// Spočítaná nová výše splátky jako ExtraPaymentAmount + FeeAmountTotal (s odečtenou slevou)
    /// </summary>
    public decimal ExtraPaymentAmountTotal { get; set; }
}
