namespace FOMS.Api.Endpoints.Offer.Dto;

public class MortgageOutputs
{
    public decimal InterestRate { get; set; }
    public decimal InterestRateAnnounced { get; set; }
    public decimal LoanAmount { get; set; }
    public int LoanDuration { get; set; }
    public decimal LoanPaymentAmount { get; set; }
    public int? EmployeeBonusLoanCode { get; set; }
    public decimal LoanToValue { get; set; }
    public decimal LoanToCost { get; set; }
    public decimal Aprc { get; set; }
    public decimal LoanTotalAmount { get; set; }
    public int StatementTypeId { get; set; }
    public List<int>? LoanPurpose { get; set; }
}
