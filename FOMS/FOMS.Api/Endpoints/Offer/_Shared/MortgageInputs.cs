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
    /// Splátka úvěru
    /// </summary>
    /// <example>15000</example>
    public decimal? LoanPaymentAmount { get; set; }
    
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
    /// Předpokládáné výše úvěru
    /// </summary>
    /// <example>300000</example>
    public decimal LoanToValue { get; set; }

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

    public List<LoanPurposeItem>? LoanPurposes { get; set; }
}
