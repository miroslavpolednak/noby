namespace FOMS.Api.Endpoints.Offer.Dto;

internal class MortgageResults
{
    public decimal InterestRate { get; set; }
    public decimal LoanAmount { get; set; }
    public int LoanDuration { get; set; }
    public int LoanPaymentAmount { get; set; }
    public int? EmployeeBonusLoanCode { get; set; }
    public decimal Ltv { get; set; }
    public decimal Aprc { get; set; }
}
