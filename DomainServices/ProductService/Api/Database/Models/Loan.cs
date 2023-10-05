namespace DomainServices.ProductService.Api.Database.Models;

internal class Loan
{
	public long Id { get; set; }
	public long? PartnerId { get; set; }
	public int? ProductTypeId { get; set; }
	public string? ContractNumber { get; set; }
	public decimal? LoanPaymentAmount { get; set; }
	public decimal? LoanAmount { get; set; }
	public decimal? LoanInterestRate { get; set; }
	public short? FixedRatePeriod { get; set; }
	public int LoanKindId { get; set; }
	public DateTime? ExpectedDateOfDrawing { get; set; }
	public DateTime? FirstSignatureDate { get; set; }
	public long? CaseOwnerUserCurrentId { get; set; }
	public long? CaseOwnerUserOrigId { get; set; }
	public decimal? AvailableForDrawing { get; set; }
	public decimal? CurrentAmount { get; set; }
	public DateTime? DrawingDateTo { get; set; }
	public DateTime? ContractSignedDate { get; set; }
	public DateTime? FixedRateValidTo { get; set; }
	public DateTime? FirstAnnuityInstallmentDate { get; set; }
	public DateTime? LoanDueDate { get; set; }
	public string? PaymentAccountNumber { get; set; }
	public string? PaymentAccountPrefix { get; set; }
	public decimal? Principal { get; set; }
	public decimal? CurrentOverdueAmount { get; set; }
	public decimal? AllOverdueFees { get; set; }
	public int? OverdueDaysNumber { get; set; }
	public decimal? InterestInArrears { get; set; }
	public int? PaymentDay { get; set; }
	public long? BranchConsultantId { get; set; }
	public string? RepaymentAccountPrefix { get; set; }
	public string? RepaymentAccountNumber { get; set; }
	public string? RepaymentAccountBankCode { get; set; }
	public short? StatementFrequencyId { get; set; }
	public short? StatementSubscriptionTypeId { get; set; }
	public short? StatementTypeId { get; set; }
	public string? EmailAddress1 { get; set; }
    public string? EmailAddress2 { get; set; }
    public bool IsCancelled { get; set; }
}