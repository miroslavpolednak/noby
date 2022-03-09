namespace FOMS.Api.Endpoints.Offer.Dto;

public class MortgageOutputs
{
    /// <summary>
    /// Skládačková úroková sazba (číselníková sazba mínus slevy dané marketingovými akcemi). 
    /// </summary>
    public decimal InterestRate { get; set; }
    
    /// <summary>
    /// Standardní číselníková úroková sazba
    /// </summary>
    public decimal InterestRateAnnounced { get; set; }
    
    /// <summary>
    /// Výše úvěru
    /// </summary>
    public decimal LoanAmount { get; set; }
    
    public int LoanDuration { get; set; }
    public decimal LoanPaymentAmount { get; set; }
    public int? EmployeeBonusLoanCode { get; set; }
    public decimal LoanToValue { get; set; }
    public decimal LoanToCost { get; set; }
    public decimal Aprc { get; set; }
    public decimal LoanTotalAmount { get; set; }
    public int StatementTypeId { get; set; }

    /// <summary>
    /// Den splátky úvěru
    /// </summary>
    public int? PaymentDayOfTheMonth { get; set; }

    public List<LoanPurposeItem>? LoanPurpose { get; set; }
}
