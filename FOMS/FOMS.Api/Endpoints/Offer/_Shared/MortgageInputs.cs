namespace FOMS.Api.Endpoints.Offer.Dto;

internal class MortgageInputs
{
    public int ProductInstanceTypeId { get; set; }
    public int LoanKindId { get; set; }
    public int LoanAmount { get; set; }
    public int LoanDuration { get; set; }
    public int LoanPaymentAmount { get; set; }
    public int FixationPeriod { get; set; }
    public int? EmployeeBonusLoanCode { get; set; }
    public int CollateralAmount { get; set; }
    public int LoanToValue { get; set; }
    public int PaymentDayOfTheMonth { get; set; }
    public bool EmployeeBonusRequested { get; set; }
    public List<int>? MarketingActions { get; set; }
    public List<int>? LoanPurpose { get; set; }
}
