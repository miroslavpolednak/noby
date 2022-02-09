namespace FOMS.Api.Endpoints.Offer.Dto;

internal class MortgageInputs
{
    public int ProductTypeId { get; set; }
    public int LoanKindId { get; set; }
    public decimal LoanAmount { get; set; }
    public int? LoanDuration { get; set; }
    public decimal LoanPaymentAmount { get; set; }
    public int FixationPeriod { get; set; }
    public int? EmployeeBonusLoanCode { get; set; }
    public decimal CollateralAmount { get; set; }
    public decimal LoanToValue { get; set; }
    public int? PaymentDayOfTheMonth { get; set; }
    public bool EmployeeBonusRequested { get; set; }
    public DateTime? ExpectedDateOfDrawing { get; set; }
    public List<int>? LoanPurpose { get; set; }
}
