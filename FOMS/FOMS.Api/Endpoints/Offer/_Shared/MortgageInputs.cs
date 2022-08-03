namespace FOMS.Api.Endpoints.Offer.Dto;

public class MortgageInputs
{
    /// <summary>
    /// ID produktu. Ciselnik ProductTypes.
    /// </summary>
    /// <example>20001</example>
    public int ProductTypeId { get; set; }
    
    /// <summary>
    /// Druh uveru. Ciselnik LoanKind.
    /// </summary>
    /// <example>0</example>
    public int LoanKindId { get; set; }
    
    /// <summary>
    /// Výše úvěru
    /// </summary>
    /// <example>1000000</example>
    public decimal? LoanAmount { get; set; }
    
    /// <summary>
    /// Splatnost úvěru
    /// </summary>
    /// <example>10</example>
    public int? LoanDuration { get; set; }
    
    /// <summary>
    /// Délka fixace úrokové sazby
    /// </summary>
    /// <example>48</example>
    public int FixedRatePeriod { get; set; }
    
    /// <summary>
    /// Předpokládáné hodnoty zajištění
    /// </summary>
    /// <example>5000000</example>
    public decimal CollateralAmount { get; set; }

    /// <summary>
    /// Den splátky úvěru
    /// </summary>
    /// <example></example>
    public int? PaymentDay { get; set; }

    /// <summary>
    /// Je žádáno zaměstnanecké zvýhodnění?
    /// </summary>
    public bool? IsEmployeeBonusRequested { get; set; }

    /// <summary>
    /// Předpokládané datum čerpání
    /// </summary>
    /// <example></example>
    public DateTime? ExpectedDateOfDrawing { get; set; }

    /// <summary>
    /// Vlastní zdroje
    /// </summary>
    public decimal? FinancialResourcesOwn { get; set; }

    /// <summary>
    /// Cizí zdroje
    /// </summary>
    public decimal? FinancialResourcesOther { get; set; }

    public int? StatementTypeId { get; set; }

    /// <summary>
    /// uver.rozhodnyDenSazby
    /// </summary>
    public DateTime GuaranteeDateFrom { get; set; }

    /// <summary>
    /// uver.indCenotvorbaOdchylka
    /// </summary>
    public decimal? InterestRateDiscount { get; set; }

    /// <summary>
    /// uver.typCerpani
    /// </summary>
    public int? DrawingType { get; set; }

    /// <summary>
    /// uver.lhutaDocerpani
    /// </summary>
    public int? DrawingDuration { get; set; }

    public List<LoanPurposeItem>? LoanPurposes { get; set; }

    public MarketingActionInputItem? MarketingActions { get; set; }

    public Developer? Developer { get; set; }

    public List<FeeInputItem>? Fees { get; set; }
}
