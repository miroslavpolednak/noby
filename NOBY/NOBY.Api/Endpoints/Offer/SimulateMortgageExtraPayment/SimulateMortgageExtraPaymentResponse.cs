namespace NOBY.Api.Endpoints.Offer.SimulateMortgageExtraPayment;

public sealed class SimulateMortgageExtraPaymentResponse
{
    /// <summary>
    /// ID vytvorene simulace.
    /// </summary>
    public int OfferId { get; set; }

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
}
